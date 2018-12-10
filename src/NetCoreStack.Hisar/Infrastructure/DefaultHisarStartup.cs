using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace NetCoreStack.Hisar
{
    public class DefaultHisarStartup<TStartup> : IStartup where TStartup : class
    {
        private readonly Assembly _componentAssembly = typeof(TStartup).GetTypeInfo().Assembly;
        private readonly HisarConventionBasedStartup _componentStartup;
        private readonly IHostingEnvironment _hostingEnvironment;

        public IConfiguration Configuration { get; }

        public DefaultHisarStartup(IServiceProvider serviceProvider, 
            IHostingEnvironment hostingEnvironment,
            IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            Configuration = Configuration;
            _componentStartup = StartupTypeLoader.CreateHisarConventionBasedStartup(typeof(TStartup), serviceProvider, _hostingEnvironment);
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddHisar<TStartup>(Configuration, _hostingEnvironment);
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