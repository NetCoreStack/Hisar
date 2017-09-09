using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace NetCoreStack.Hisar
{
    public class DefaultHisarStartup<TStartup> : IStartup where TStartup : class
    {
        private readonly Assembly _componentAssembly = typeof(TStartup).GetTypeInfo().Assembly;
        private readonly HisarConventionBasedStartup _componentStartup;
        private readonly IHostingEnvironment _env;
        private readonly ILoggerFactory _loggerFactory;
        public IConfigurationRoot Configuration { get; }

        public DefaultHisarStartup(IServiceProvider sp, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            _env = env;
            _loggerFactory = loggerFactory;
            _componentStartup = StartupTypeLoader.CreateHisarConventionBasedStartup(typeof(TStartup), sp, _env);
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddHisar<TStartup>(Configuration, _env);
            _componentStartup.ConfigureServices(services);
            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app)
        {
            _componentStartup.Configure(app);
            app.UseHisar<TStartup>();
        }
    }
}