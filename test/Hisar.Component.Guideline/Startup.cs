using Hisar.Component.Guideline.Filters;
using NetCoreStack.Hisar;
using NetCoreStack.Hisar.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreStack.Data.Context;
using System.IO;
using NetCoreStack.WebSockets.ProxyClient;
using Hisar.Component.Guideline.Core;

namespace Hisar.Component.Guideline
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
#if DEBUG
            services.AddProxyWebSockets(options => {
                options.ConnectorName = $"{nameof(Guideline)}-Component";
                options.WebSocketHostAddress = "localhost:1444"; // Hisar WebCLI default socket
                options.RegisterInvocator<DataStreamingInvocator>(NetCoreStack.WebSockets.WebSocketCommands.All);
            });
#endif

            services.AddHisarMongoDbContext<MongoDbContext>(Configuration);

            services.AddMvc();
            services.AddSingleton<ILayoutFilter, GuidelineLayoutWebPackFilter>();
        }
        
        public void Configure(IApplicationBuilder app)
        {
            app.UseProxyWebSockets();
            app.UseMvc(ConfigureRoutes);
        }

        public static void ConfigureRoutes(IRouteBuilder routes)
        {
            routes.MapRoute(
                name: "req",
                template: "registration",
                defaults: new { controller = "Home", action = "Registration" });

            routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}");
        }

        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<DefaultHisarStartup<Startup>>()
                .Build();

            host.Run();
        }
    }
}