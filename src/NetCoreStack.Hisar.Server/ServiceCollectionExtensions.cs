using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreStack.Data;
using NetCoreStack.Data.Context;

namespace NetCoreStack.Hisar.Server
{
    public static class ServiceCollectionExtensions
    {
        public static void AddHisarMongoDbContext<TContext>(this IServiceCollection services, 
            IConfiguration configuration) where TContext : MongoDbContext
        {
            services.AddSingleton<ICollectionNameSelector, HisarCollectionNameSelector>();
            services.AddNetCoreStackMongoDb<TContext>(configuration);
        }
    }
}
