using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreStack.Data;
using NetCoreStack.Data.Context;

namespace NetCoreStack.Hisar.Server
{
    public static class ServiceCollectionExtensions
    {
        public static void AddHisarBsonContext<TContext>(this IServiceCollection services, 
            IConfigurationRoot configuration) where TContext : MongoDbContext
        {
            services.AddNetCoreStackBsonDb<TContext>(configuration);
        }
    }
}
