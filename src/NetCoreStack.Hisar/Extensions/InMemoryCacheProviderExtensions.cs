using NetCoreStack.Contracts;
using System;
using System.Threading.Tasks;

namespace NetCoreStack.Hisar
{
    public static class InMemoryCacheProviderExtensions
    {        
        public static TModel Set<TModel>(this IMemoryCacheProvider cacheProvider, object id, TModel value)
        {
            var cacheItem = cacheProvider.ModelKeyGenerator.CreateCacheItem<TModel>(id);
            return cacheProvider.Set(cacheItem, value);
        }

        public static TModel Set<TModel>(this IMemoryCacheProvider cacheProvider, object id, TModel value, DateTimeOffset? absoluteExpiration)
        {
            var cacheItem = cacheProvider.ModelKeyGenerator.CreateCacheItem<TModel>(id);
            cacheItem.AbsoluteExpiration = absoluteExpiration;
            return cacheProvider.Set(cacheItem, value);
        }

        public static async Task<TModel> GetOrCreateAsync<TModel, TKey>(this IMemoryCacheProvider cacheProvider, 
            TKey id, 
            Func<TKey, CacheItem, Task<TModel>> factory,
            DateTimeOffset? absoluteExpiration = null)
             where TModel : IModelKey<TKey>
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            var cacheItem = cacheProvider.ModelKeyGenerator.CreateCacheItem<TModel>(id);
            if (typeof(TKey) == typeof(String))
            {
                if(cacheProvider.TryGetValue(cacheItem, out IModelKey<string> value))
                {
                    return (TModel)value;
                }
            }

            if (typeof(TKey).IsNumericType())
            {
                if (cacheProvider.TryGetValue(cacheItem, out IModelKey<long> value))
                {
                    return (TModel)value;
                }
            }

            var instance = await factory.Invoke(id, cacheItem);
            return Set(cacheProvider, id, instance, absoluteExpiration);
        }
    }
}
