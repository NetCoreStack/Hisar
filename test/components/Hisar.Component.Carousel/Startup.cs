using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreStack.Hisar;
using Shared.Library;
using System.IO;

namespace Hisar.Component.Carousel
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
#if !RELEASE
            services.AddWebCliSocket<Startup>();
#endif

            services.AddMenuRenderer<SharedMenuItemsRenderer>();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
#if !RELEASE
            app.UseCliProxy();
#endif

            app.UseMvc(ConfigureRoutes);
        }

        public static void ConfigureRoutes(IRouteBuilder routes)
        {
            routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}");
        }

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<DefaultHisarStartup<Startup>>();
    }
}
