using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCoreStack.Data.Context;
using NetCoreStack.Hisar;
using NetCoreStack.Hisar.Server;
using NetCoreStack.Mvc;

namespace Hisar.Component.ContentManagement
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
#if !RELEASE
            services.AddWebCliSocket<Startup>();
#endif
            services.AddHisarMongoDbContext<MongoDbContext>(Configuration);

            services.AddComposers(options =>
            {
                options.CreateMap<ContentObjectViewModel, IcerikViewModelComposer>();
            });

            services.AddCacheValueProviders(setup =>
            {
                setup.DefaultMap<ContentObjectViewModel, ContentCacheValueProvider>();
            });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
#if !RELEASE
            app.UseCliProxy();
#endif
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
