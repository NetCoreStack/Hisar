using NetCoreStack.Hisar;
using NetCoreStack.Hisar.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCoreStack.Data.Context;
using System;
using System.IO;

namespace Landing.Hosting
{
    public class Startup
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHostingEnvironment _env;
        public IConfigurationRoot Configuration { get; }

        public Startup(IServiceProvider serviceProvider, IHostingEnvironment env)
        {
            _serviceProvider = serviceProvider;
            _env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHisarMongoDbContext<MongoDbContext>(Configuration);
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            if (!_env.IsProduction())
            {
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug();

                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
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

        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddCommandLine(args).Build();

            var host = new WebHostBuilder()
                .UseConfiguration(configuration)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<DefaultHisarStartup<Startup>>()
                .Build();

            host.Run();
        }
    }
}