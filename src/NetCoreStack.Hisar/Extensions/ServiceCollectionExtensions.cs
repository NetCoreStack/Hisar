using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NetCoreStack.Mvc;
using NetCoreStack.Mvc.Helpers;
using NetCoreStack.Mvc.Interfaces;
using NetCoreStack.WebSockets;
using NetCoreStack.WebSockets.ProxyClient;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NetCoreStack.Hisar
{
    public static class ServiceCollectionExtensions
    {
        private static HisarAssemblyComponentsLoader CreateAssemblyLoader(IServiceCollection services, IHostingEnvironment env, IMvcBuilder builder)
        {
            var assemblyLoader = new HisarAssemblyComponentsLoader(services.BuildServiceProvider(), env);
            assemblyLoader.LoadComponents(services, builder);
            services.AddSingleton(assemblyLoader);

            return assemblyLoader;
        }

        internal static RunningComponentHelperOfT<TStartup> CreateComponentHelper<TStartup>(this IServiceCollection services)
        {
            var serviceDescriptor = services.FirstOrDefault(x => x.ServiceType == typeof(IComponentTypeResolver));
            return new RunningComponentHelperOfT<TStartup>(serviceDescriptor.CreateInstance<IComponentTypeResolver>());
        }

        internal static void AddHisar<TStartup>(this IServiceCollection services, 
            IConfigurationRoot configuration, 
            IHostingEnvironment env) where TStartup : class
        {
            services.AddNetCoreStackMvc();
            services.AddSingleton<HisarMarkerService>();

            // Singletons
            services.TryAddSingleton<ICommonCacheProvider, InMemoryCacheProvider>();
            services.TryAddSingleton<IJsonSerializer, JsonSerializer>();
            services.TryAddSingleton<IDataProtectorProvider, HisarDataProtectorProvider>();
            services.TryAddSingleton<IComponentTypeResolver, DefaultComponentTypeResolver>();
            services.TryAddSingleton<IAssemblyFileProviderFactory, DefaultAssemblyFileProviderFactory>();

            // Custom view component helper
            services.AddTransient<IViewComponentHelper, HisarDefaultViewComponentHelper>();

            // Per request services
            services.TryAddScoped<IUrlGeneratorHelper, UrlGeneratorHelper>();
            services.TryAddScoped<IMenuItemsRenderer, DefaultMenuItemsRenderer>();

            // New instances
            services.TryAddTransient<IHisarExceptionFilter, DefaultHisarExceptionFilter>();            
            services.TryAddTransient<IHisarCacheValueProvider, HisarDefaultCacheValueProvider>();

            var componentHelper = CreateComponentHelper<TStartup>(services);
            var assembly = typeof(TStartup).GetTypeInfo().Assembly;

            services.TryAddScoped<IMenuItemsBuilder, DefaultMenuItemsBuilder<TStartup>>();


            bool isComponent = componentHelper.IsExternalComponent;
            bool isCoreComponent = componentHelper.IsCoreComponent;
            IMvcBuilder builder = null;
            if (isComponent)
            {
                builder = services.AddMvc(options =>
                {
                    options.Filters.Add(new HisarExceptionFilter());
                    if (isCoreComponent)
                    {
                        options.Conventions.Add(new NameSpaceRoutingConvention(componentHelper));
                    }
                });
            }
            else
            {
                services.AddSingleton<IViewComponentSelector, HostingViewComponentSelector>();

                builder = services.AddMvc(options => {
                    options.Filters.Add(new HisarExceptionFilter());
                    options.Conventions.Add(new NameSpaceRoutingConvention(componentHelper));
                });
            }

            builder.ConfigureApplicationPartManager(manager => manager.ApplicationParts.Clear())
                .AddApplicationPart(assembly);

            builder.AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddSingleton(componentHelper as RunningComponentHelper);

            HisarAssemblyComponentsLoader assemblyLoader = null;
            if (isComponent)
            {
                if (isCoreComponent)
                    assemblyLoader = CreateAssemblyLoader(services, env, builder);

                var defaultLayoutFileProvider = new DefaultProxyFileLocator();
                services.TryAddSingleton<IDefaultProxyFileLocator>(_ => defaultLayoutFileProvider);
                builder.AddRazorOptions(options =>
                {
                    options.FileProviders.Add(new InMemoryCliFileProvider(defaultLayoutFileProvider));
                    var peRef = MetadataReference.CreateFromFile(assembly.Location);
                    options.AdditionalCompilationReferences.Add(peRef);

                    // component formats
                    options.AreaViewLocationFormats.Add("/Views/{1}/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Views/{1}/{0}.cshtml");
                });
            }
            else
            {
                assemblyLoader = CreateAssemblyLoader(services, env, builder);
                builder.AddRazorOptions(options =>
                {
                    options.FileProviders.Add(new HisarEmbededFileProvider(assemblyLoader.ComponentAssemblyLookup));
                    foreach (KeyValuePair<string, Assembly> entry in assemblyLoader.ComponentAssemblyLookup)
                    {
                        var nameSpace = entry.Value.GetName().Name;
                        var peRef = MetadataReference.CreateFromFile(entry.Value.Location);
                        options.AdditionalCompilationReferences.Add(peRef);
                    }
                });
            }

            services.AddSingleton(_ => services);
        }
    }

    public static class ServiceCollectionExtensionsProxy
    {
        public static void AddCliSocket<TStartup>(this IServiceCollection services)
        {
            var executingAssembly = Assembly.GetEntryAssembly();
            var componentHelper = services.CreateComponentHelper<TStartup>();
            if (!executingAssembly.EnsureIsHosting()) // Running as standalone not part of any Hosting
            {
                services.AddSingleton<CliUsageMarkerService>();

                services.AddProxyWebSockets(options => {
                    options.ConnectorName = $"{nameof(componentHelper.ComponentId)}-Component";
                    options.WebSocketHostAddress = "localhost:1444"; // Hisar WebCLI default socket
                    options.RegisterInvocator<DataStreamingInvocator>(WebSocketCommands.All);
                });
            }
        }
    }
}