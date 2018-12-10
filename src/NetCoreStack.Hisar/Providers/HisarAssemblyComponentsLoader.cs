using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
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

        private void RegisterComponent(IServiceCollection services, IMvcBuilder builder, 
            Assembly assembly, 
            List<HisarCacheAttribute> cacheItems)
        {
            var assemblyName = assembly.GetName().Name;
            var componentId = assembly.GetComponentId();
            ComponentAssemblyLookup.Add(componentId, assembly);

            AssemblyLoadContext.Default.Resolving += DefaultResolving;

            if (cacheItems != null)
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
            finally
            {
                AssemblyLoadContext.Default.Resolving -= null;
            }

            var manager = builder.PartManager;
            var assemblyPart = new AssemblyPart(assembly);
            if (!manager.ApplicationParts.Contains(assemblyPart))
            {
                manager.ApplicationParts.Add(assemblyPart);
            }
        }

        private Assembly DefaultResolving(AssemblyLoadContext loadContext, AssemblyName assemblyName)
        {
            if (assemblyName.Name == typeof(HisarAssemblyComponentsLoader).Assembly.GetName().Name)
            {
                return typeof(HisarAssemblyComponentsLoader).Assembly;
            }

            return null;
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

        protected virtual void LoadComponentsJson(IServiceCollection services,
            IMvcBuilder builder,
            List<HisarCacheAttribute> cacheItems,
            string componentJsonFilePath, 
            string externalComponentsRefDirectory)
        {
            if (string.IsNullOrEmpty(componentJsonFilePath))
            {
                throw new ArgumentNullException(nameof(componentJsonFilePath));
            }

            if (!File.Exists(componentJsonFilePath))
            {
                throw new FileNotFoundException(nameof(componentJsonFilePath));
            }

            ComponentsJson json = null;
            using (StreamReader file = File.OpenText(componentJsonFilePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                json = (ComponentsJson)serializer.Deserialize(file, typeof(ComponentsJson));
                if (json == null || json.Components == null)
                {
                    return;
                }
            }

            foreach (KeyValuePair<string, ComponentJsonDefinition> entry in json.Components)
            {
                var componentId = entry.Key.GetComponentId();
                if (ComponentAssemblyLookup.ContainsKey(componentId))
                    continue; // local wins

                var packageId = entry.Key;
                var targetFramework = entry.Value.TargetFramework;
                var version = entry.Value.Version;

                var assembly = NugetHelper.TryLoadFromNuget(externalComponentsRefDirectory, targetFramework, packageId, version);
                if (assembly != null)
                {
                    RegisterComponent(services, builder, assembly, cacheItems);
                }
            }
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

            // Local packages loader
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
                        var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(fullPath);
                        ReferencedAssembliesResolver.ResolveAssemblies(GetResolveCallback(), externalComponentsRefDirectory, assembly);
                        RegisterComponent(services, builder, assembly, cacheItems);
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
                                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(fullPath);
                                builder.PartManager.ApplicationParts.Add(new AssemblyPart(assembly));
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
            }

            var componentsJsonPath = PathUtility.TryGetComponentsJson(externalComponentsDirectory);
            if (!string.IsNullOrEmpty(componentsJsonPath))
            {
                LoadComponentsJson(services, builder, cacheItems, componentsJsonPath, externalComponentsRefDirectory);
            }

            services.AddSingleton<ICacheItemResolver>(new DefaultCacheItemResolver(cacheItems));

            services.AddImplementations<IUsernamePasswordValidator>(ComponentAssemblyLookup);
            services.AddImplementations<IUserRegistration>(ComponentAssemblyLookup);
        }
    }
}