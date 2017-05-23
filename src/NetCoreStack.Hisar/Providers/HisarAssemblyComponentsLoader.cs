using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace NetCoreStack.Hisar
{
    public class HisarAssemblyComponentsLoader
    {
        public static readonly string ComponentConventionBaseNamespace = "Hisar.Component";
        private const string ControllerTypeNameSuffix = "Controller";

        private readonly IHostingEnvironment _env;
        public IDictionary<string, Assembly> ComponentAssemblyLookup { get; set; }
        public IDictionary<string, HisarConventionBasedStartup> StartupLookup { get; set; }
        protected IServiceProvider ServiceProvider { get; }

        private IAssemblyProviderResolveCallback GetResolveCallback()
        {
            return ServiceProviderServiceExtensions.GetService<IAssemblyProviderResolveCallback>(ServiceProvider);
        }

        public HisarAssemblyComponentsLoader(IServiceProvider serviceProvider, IHostingEnvironment env)
        {
            ServiceProvider = serviceProvider;
            _env = env;
            ComponentAssemblyLookup = new Dictionary<string, Assembly>(StringComparer.OrdinalIgnoreCase);
            StartupLookup = new Dictionary<string, HisarConventionBasedStartup>();
        }

        protected virtual bool IsController(TypeInfo typeInfo)
        {
            if (!typeInfo.IsClass)
            {
                return false;
            }

            if (typeInfo.IsAbstract)
            {
                return false;
            }

            if (!typeInfo.IsPublic)
            {
                return false;
            }

            if (typeInfo.ContainsGenericParameters)
            {
                return false;
            }

            if (typeInfo.IsDefined(typeof(NonControllerAttribute)))
            {
                return false;
            }

            if (!typeInfo.Name.EndsWith(ControllerTypeNameSuffix, StringComparison.OrdinalIgnoreCase) &&
                !typeInfo.IsDefined(typeof(ControllerAttribute)))
            {
                return false;
            }

            return true;
        }

        public virtual void LoadComponents(IServiceCollection services, IMvcBuilder builder)
        {
            var entityList = new List<Type>();
            var cacheItems = new List<HisarCacheAttribute>();

            var externalComponentsDirectory = Path.GetFullPath("ExternalComponents");
            if (!Directory.Exists(externalComponentsDirectory))
            {
                Directory.CreateDirectory(externalComponentsDirectory);
            }

            var externalComponentsRefDirectory = Path.Combine(externalComponentsDirectory, "refs");
            PathUtility.CopyToFiles(externalComponentsDirectory, externalComponentsRefDirectory);

            var fileFullPaths = Directory.GetFiles(externalComponentsRefDirectory);
            if (fileFullPaths != null && fileFullPaths.Any())
            {
                foreach (var fileFullPath in fileFullPaths)
                {
                    var fileName = Path.GetFileName(fileFullPath);
                    if (fileName.StartsWith(ComponentConventionBaseNamespace, StringComparison.OrdinalIgnoreCase) &&
                        Path.GetExtension(fileName) == ".dll")
                    {
                        var fullPath = Path.GetFullPath(fileFullPath);

                        // AssemblyLoadContext.Default.Resolving += ReferencedAssembliesResolver.Resolving;
                        var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(fullPath);
                        ReferencedAssembliesResolver.ResolveAssemblies(GetResolveCallback(), externalComponentsRefDirectory, assembly);
                        var assemblyName = assembly.GetName().Name;
                        var componentId = assembly.GetComponentId();
                        ComponentAssemblyLookup.Add(componentId, assembly);

                        cacheItems.AddRange(assembly.GetTypesAttributes<HisarCacheAttribute>());
                        try
                        {
                            var startupType = StartupLoader.FindStartupType(assemblyName, _env.EnvironmentName);
                            if (startupType != null)
                            {
                                var startup = StartupTypeLoader.CreateHisarConventionBasedStartup(startupType, ServiceProvider, _env);
                                StartupLookup.Add(componentId, startup);
                                startup.ConfigureServices(services);

                                services.AddMenuBuilders(startupType);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }

                        var components = assembly.GetTypes().ToArray();
                        var controllers = components.Where(c => IsController(c.GetTypeInfo())).ToList();
                        builder.PartManager.ApplicationParts.Add(new TypesPart(components));
                    }
                    else
                    {
                        var filename = Path.GetFileName(fileFullPath);
                        if (!filename.Contains("NetCoreStack.Hisar") && filename.EndsWith(".dll"))
                        {
                            var fullPath = Path.GetFullPath(fileFullPath);
                            var entryAssembly = Assembly.GetEntryAssembly();

                            var assemblyName = Path.GetFileNameWithoutExtension(filename);
                            if (entryAssembly.GetReferencedAssemblies().Any(x => x.Name == assemblyName))
                                continue;

                            try
                            {
                                AssemblyLoadContext.Default.LoadFromAssemblyPath(fullPath);
                            }
                            catch (Exception ex)
                            {
                                var resolved = GetResolveCallback().TryLoad(AssemblyLoadContext.Default, entryAssembly, fullPath, ex);
                                if (!resolved)
                                    throw new FileLoadException($"{fullPath} could not be loaded! [Nuget Source]");
                            }
                        }
                    }
                }

                services.AddSingleton<ICacheItemResolver>(new DefaultCacheItemResolver(cacheItems));
            }

            // Validatiors
            var validators = ComponentAssemblyLookup.Values.SelectMany(a => a.GetTypes()).
                Where(x => typeof(IUsernamePasswordValidator).IsAssignableFrom(x)).ToList();

            if (validators.Count > 1)
            {
                throw new InvalidOperationException($"{nameof(IUsernamePasswordValidator)} type registered more then once!");
            }

            services.AddScoped(typeof(IUsernamePasswordValidator), validators[0]);
        }
    }
}