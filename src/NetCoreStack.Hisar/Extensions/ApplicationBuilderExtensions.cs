using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace NetCoreStack.Hisar
{
    public static class ApplicationBuilderExtensions
    {
        private static RunningComponentHelper GetComponentHelper(IServiceProvider applicationServices)
        {
            return ServiceProviderServiceExtensions.GetService<RunningComponentHelper>(applicationServices);
        }

        public static IApplicationBuilder UseHisar<TStartup>(this IApplicationBuilder app)
        {
            ThrowIfServiceNotRegistered(app.ApplicationServices);
            app.UseStaticFiles();

            var componentHelper = GetComponentHelper(app.ApplicationServices);
            bool isExternalComponent = componentHelper.IsExternalComponent;
            if (isExternalComponent)
            {
                // noop
            }
            else
            {
                // for registered components
                var assemblyLoader = app.ApplicationServices.GetRequiredService<HisarAssemblyComponentsLoader>();
                foreach (var lookup in assemblyLoader.ComponentAssemblyLookup)
                {
                    app.UseStaticFiles(new StaticFileOptions()
                    {
                        FileProvider = new ComponentFileProvider(lookup.Value),
                        RequestPath = $"/{lookup.Key}"
                    });
                }

                var componentsRoutesDictionary = new Dictionary<string, IList<IRouter>>();
                foreach (KeyValuePair<string, HisarConventionBasedStartup> entry in assemblyLoader.StartupLookup)
                {
                    var componentId = entry.Key;
                    var startup = entry.Value;
                    startup.Configure(app);

                    // unique builder - route name handler
                    var componentRoutesBuilder = new RouteBuilder(app)
                    {
                        DefaultHandler = app.ApplicationServices.GetRequiredService<MvcRouteHandler>(),
                    };

                    startup.ConfigureRoutes(componentRoutesBuilder);
                    componentsRoutesDictionary.Add(entry.Key, componentRoutesBuilder.Routes);
                }                

                app.UseMvc(routes =>
                {
                    ConfigureRoutes(routes, componentsRoutesDictionary);

                    // add hosting component default routes
                    var startupType = typeof(TStartup);
                    var configureRoutes = StartupTypeLoader.GetConfigureRoutesMethod(startupType);
                    configureRoutes?.Invoke(null, new object[] { routes });
                });
            }

            return app;
        }

        private static void ConfigureRoutes(IRouteBuilder mvcRoutesBuilder, 
            Dictionary<string, IList<IRouter>> componentsRoutesDictionary)
        {
            foreach (KeyValuePair<string, IList<IRouter>> entry in componentsRoutesDictionary)
            {
                var componentId = entry.Key;
                for (int i = 0; i < entry.Value.Count; i++)
                {
                    var route = entry.Value[i] as Route;
                    var isDefaultRoute = route.IsDefaultRoute();
                    if (isDefaultRoute)
                        continue;

                    var defaults = route.Defaults as IDictionary<string, object>;
                    var template = route.RouteTemplate;

                    object areaKeyValue = null;
                    if (!defaults.TryGetValue("area", out areaKeyValue))
                    {
                        defaults.Add("area", componentId);
                    }

                    if (template.StartsWith("{"))
                    {
                        template = "{area = " + componentId + "}/" + template;
                        defaults = new Dictionary<string, object>();
                    }
                    else
                    {
                        var index = i + 1;
                        template = $"{template}-{index}";
                    }

                    mvcRoutesBuilder.MapRoute($"{componentId}{route.Name}", template, defaults);
                }
            }
        }

        private static void ThrowIfServiceNotRegistered(IServiceProvider applicationServices)
        {
            var service = applicationServices.GetService<HisarMarkerService>();
            if (service == null)
                throw new InvalidOperationException(string.Format("Required services are not registered - are you missing a call to AddHisar?"));
        }
    }
}