using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using NetCoreStack.Mvc;
using NetCoreStack.Mvc.Helpers;
using NetCoreStack.Mvc.Interfaces;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Reflection;

namespace NetCoreStack.Hisar
{
    internal static class ServiceCollectionExtensions
    {
        internal static void AddHisar<TStartup>(this IServiceCollection services, IConfigurationRoot configuration, IHostingEnvironment env) where TStartup : class
        {
            services.AddNetCoreStackMvc();
            services.AddSingleton<HisarMarkerService>();

            // Singletons
            services.TryAddSingleton<ICommonCacheProvider, InMemoryCacheProvider>();
            services.TryAddSingleton<IJsonSerializer, JsonSerializer>();
            services.TryAddSingleton<IDataProtectorProvider, HisarDataProtectorProvider>();
            services.TryAddSingleton<ILayoutFilter, DefaultLayoutFilter>();
            services.TryAddSingleton<ILayoutFactory, DefaultLayoutFactory>();
            services.TryAddSingleton<ITemplateProvider, DefaultTemplateProvider>();
            services.TryAddSingleton<IDataProtectorProvider, HisarDataProtectorProvider>();

            // Per request services
            services.TryAddScoped<IUrlGeneratorHelper, UrlGeneratorHelper>();

            // New instances
            services.TryAddTransient<IHisarExceptionFilter, DefaultHisarExceptionFilter>();
            //services.TryAddTransient<HisarViewComponentHelper>();
            services.TryAddTransient<IViewComponentHelper, HisarDefaultViewComponentHelper>();
            services.TryAddTransient<IHisarCacheValueProvider, HisarDefaultCacheValueProvider>();

            var componentHelper = new RunningComponentHelperOfT<TStartup>();

            var assembly = typeof(TStartup).GetTypeInfo().Assembly;
            bool isComponent = componentHelper.IsExternalComponent;
            IMvcBuilder builder = null;
            if (isComponent)
            {
                builder = services.AddMvc(options =>
                {
                    options.Filters.Add(new HisarExceptionFilter());
                    options.Conventions.Add(new NameSpaceRoutingConvention());
                });
            }
            else
            {
                builder = services.AddMvc(options => {
                    options.Filters.Add(new HisarExceptionFilter());
                });
            }

            builder.ConfigureApplicationPartManager(manager => manager.ApplicationParts.Clear())
                .AddApplicationPart(assembly);

            builder.AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddSingleton(componentHelper as RunningComponentHelper);

            if (isComponent)
            {
                var defaultLayoutFileProvider = new DefaultLayoutFileProvider();
                services.TryAddSingleton<IDefaultLayoutFileProvider>(_ => defaultLayoutFileProvider);
                builder.AddRazorOptions(options =>
                {
                    options.FileProviders.Add(new DefaultFileProvider(defaultLayoutFileProvider));
                    // options.FileProviders.Add(new MockEmbedFileProvider(typeof(DefaultHisarStartup<TStartup>).GetTypeInfo().Assembly));
                    var peRef = MetadataReference.CreateFromFile(assembly.Location);
                    options.AdditionalCompilationReferences.Add(peRef);

                    // component formats
                    options.AreaViewLocationFormats.Add("/Views/{1}/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Views/{1}/{0}.cshtml");
                });
            }
            else
            {
                var assemblyLoader = new HisarAssemblyComponentsLoader(services.BuildServiceProvider(), env);
                assemblyLoader.LoadComponents(services, builder);
                services.AddSingleton(assemblyLoader);

                builder.AddRazorOptions(options =>
                {
                    options.FileProviders.Add(new CompositeFileProvider(assemblyLoader.EmbeddedFileProviders));

                    foreach (KeyValuePair<string, Assembly> entry in assemblyLoader.ComponentAssemblyLookup)
                    {
                        var peRef = MetadataReference.CreateFromFile(entry.Value.Location);
                        options.AdditionalCompilationReferences.Add(peRef);

                        var namespaceExpander = entry.Value.GetName().Name.Replace(".", "/");
                        options.AreaViewLocationFormats.Add("/" + entry.Key + "/Views/{1}/{0}.cshtml");
                        options.AreaViewLocationFormats.Add("/" + entry.Key + "/Views/Shared/{0}.cshtml");

                        options.ViewLocationFormats.Add("/" + entry.Key + "/Views/{1}/{0}.cshtml");
                        options.ViewLocationFormats.Add("/" + entry.Key + "/Views/Shared/{0}.cshtml");

                        // component formats order!
                        options.AreaViewLocationFormats.Add("/Views/{1}/{0}.cshtml");
                        options.ViewLocationFormats.Add("/Views/{1}/{0}.cshtml");
                    }
                });
            }

            services.AddSingleton(_ => services);
        }
    }
}