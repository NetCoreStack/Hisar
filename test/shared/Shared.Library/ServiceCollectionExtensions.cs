using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using NetCoreStack.Hisar;
using System.Reflection;

namespace Shared.Library
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSharedFeatures(this IServiceCollection services)
        {
            var sharedAssembly = typeof(ServiceCollectionExtensions).GetTypeInfo().Assembly;
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.FileProviders.Add(new SharedEmbeddedFileProvider(sharedAssembly));
            });

            services.AddMvc().ConfigureApplicationPartManager(manager =>
            {
                var assemblyPart = new AssemblyPart(sharedAssembly);
                if (!manager.ApplicationParts.Contains(assemblyPart))
                {
                    manager.ApplicationParts.Add(assemblyPart);
                }
            });
        }

        public static void UseSharedFeatures(this IApplicationBuilder app)
        {
        }
    }
}
