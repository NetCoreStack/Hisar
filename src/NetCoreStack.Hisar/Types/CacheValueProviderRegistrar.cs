using Microsoft.Extensions.DependencyInjection;
using NetCoreStack.Contracts;

namespace NetCoreStack.Hisar
{
    public class CacheValueProviderRegistrar
    {
        private readonly IServiceCollection _services;
        public CacheValueProviderRegistrar(IServiceCollection services)
        {
            _services = services;
        }

        public void DefaultMap<TModel, TValueProvider>() where TValueProvider : CacheValueProviderBase<TModel> where TModel : IModelKey<string>
        {
            _services.AddScoped<ICacheValueProvider<TModel>, TValueProvider>();
        }

        public void ModelMap<TModel, TValueProvider>() where TValueProvider : CacheValueProviderBase<TModel> where TModel : IModelKey<long>
        {
            _services.AddScoped<ICacheValueProvider<TModel>, TValueProvider>();
        }
    }
}
