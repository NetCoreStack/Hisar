using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCoreStack.Hisar;
using Shared.Library;

namespace Admin.Hosting
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMenuRenderer<SharedMenuItemsRenderer>();
            services.AddSingleton<IAssemblyProviderResolveCallback, CustomAssemblyResolveCallback>();
            services.AddMvc();

            var registryAssemblyPath = typeof(Microsoft.Win32.Registry).Assembly.Location;
        }

        public static void ConfigureRoutes(IRouteBuilder routes)
        {
            routes.MapRoute(
                name: "areaDefault",
                template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}");
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseMvc(ConfigureRoutes);
        }
    }
}
