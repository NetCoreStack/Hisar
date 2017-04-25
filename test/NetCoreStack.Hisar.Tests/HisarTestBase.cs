using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using NetCoreStack.Hisar.Tests.Core;
using System;
using System.Diagnostics;
using System.IO;

namespace NetCoreStack.Hisar.Tests
{
    public class HisarTestBase : IDisposable
    {
        public IServiceProvider ApplicationServices { get; }
        public IHostingEnvironment HostingEnvironment { get; }
        public HisarTestBase()
        {
            IServiceCollection services = new ServiceCollection();

            HostingEnvironment = new HostingEnvironment
            {
                ContentRootPath = Directory.GetCurrentDirectory(),
                EnvironmentName = EnvironmentName.Development
            };

            var loggerFactory = new LoggerFactory();

            services.AddSingleton<IComponentTypeResolver, TestComponentTypeResolver>();
            services.AddSingleton<IServiceProviderFactory<IServiceCollection>, DefaultServiceProviderFactory>();
            services.AddSingleton<ILoggerFactory>(loggerFactory);

            services.AddSingleton<DiagnosticSource>(new DiagnosticListener("Microsoft.AspNetCore.Mvc"));
            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();

            var startup = new DefaultHisarStartup<Startup>(services.BuildServiceProvider(), HostingEnvironment, loggerFactory);
            startup.ConfigureServices(services);
            ApplicationServices = services.BuildServiceProvider();
            var applicationBuilder = new ApplicationBuilder(ApplicationServices);
            startup.Configure(applicationBuilder);
        }

        public void Dispose()
        {
        }
    }
}
