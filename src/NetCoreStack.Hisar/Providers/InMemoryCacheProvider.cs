using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NetCoreStack.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreStack.Hisar
{
    public class InMemoryCacheProvider : ICommonCacheProvider
    {
        private static object _lockObject = new object();

        private readonly IDictionary<CacheItem, IModelKey<string>> LookupOne 
            = new Dictionary<CacheItem, IModelKey<string>>();

        private readonly IDictionary<CacheItem, IModelKey<long>> LookupTwo 
            = new Dictionary<CacheItem, IModelKey<long>>();

        private readonly IDictionary<string, object> Lookup = new Dictionary<string, object>();

        private readonly IModelKeyGenerator _keyGenerator;

        public InMemoryCacheProvider(IModelKeyGenerator keyGenerator)
        {
            _keyGenerator = keyGenerator;
        }
        
        private IEnumerable<ICacheValueProvider> GetValueProviders(ActionContext context)
        {
            return context.HttpContext.RequestServices.GetServices<ICacheValueProvider>();
        }

        private TModel TryGetValue<TModel, TKey>(ActionContext context, TKey id, CacheItem key) where TModel : IModelKey<TKey>
        {
            var cacheValueProviders = GetValueProviders(context);
            if (cacheValueProviders != null)
            {
                foreach (var provider in cacheValueProviders)
                {
                    var instance = provider.TryGetValue<TModel>(context, id, key);
                    if (instance != null)
                    {
                        if (instance is IModelKey<string>)
                        {
                            LookupOne.Remove(key);
                            LookupOne.Add(key, (IModelKey<string>)instance);
                            return (TModel)instance;
                        }

                        if (instance is IModelKey<long>)
                        {
                            LookupOne.Remove(key);
                            LookupTwo.Add(key, (IModelKey<long>)instance);
                            return (TModel)instance;
                        }

                        throw new NotSupportedException(nameof(TModel));
                    }
                }
            }

            return default(TModel);
        }

        public TModel GetOrCreate<TModel>(ActionContext context, long id) where TModel : IModelKey<long>
        {
            var cacheItem = _keyGenerator.CreateCacheItem<TModel>(id);
            if (LookupTwo.TryGetValue(cacheItem, out IModelKey<long> item))
                return (TModel)item;

            lock (_lockObject)
            {
                // cache was empty before we got the lock, check again inside the lock
                if (LookupTwo.TryGetValue(cacheItem, out IModelKey<long> item2))
                    return (TModel)item2;

                return TryGetValue<TModel, long>(context, id, cacheItem);
            }
        }

        public TModel GetOrCreate<TModel>(ActionContext context, string id) where TModel : IModelKey<string>
        {
            var cacheItem = _keyGenerator.CreateCacheItem<TModel>(id);
            if (LookupOne.TryGetValue(cacheItem, out IModelKey<string> item))
            {
                cacheItem = LookupOne.Keys.FirstOrDefault(x => x == cacheItem);
                if (cacheItem.AbsoluteExpiration.HasValue)
                {
                    if (cacheItem.AbsoluteExpiration.Value < DateTime.Now)
                    {
                        lock (_lockObject)
                        {
                            return TryGetValue<TModel, string>(context, id, cacheItem);
                        }
                    }
                }

                return (TModel)item;
            }

            lock (_lockObject)
            {
                // cache was empty before we got the lock, check again inside the lock
                if (LookupOne.TryGetValue(cacheItem, out IModelKey<string> item2))
                    return (TModel)item2;

                return TryGetValue<TModel, string>(context, id, cacheItem);
            }
        }

        public void Remove<TModel>(string id) where TModel : IModelKey<string>
        {
            var cacheItem = _keyGenerator.CreateCacheItem<TModel>(id);
            if (LookupOne.ContainsKey(cacheItem))
                LookupOne.Remove(cacheItem);
        }

        public void Remove<TModel>(long id) where TModel : IModelKey<long>
        {
            var cacheItem = _keyGenerator.CreateCacheItem<TModel>(id);
            if (LookupTwo.ContainsKey(cacheItem))
                LookupTwo.Remove(cacheItem);
        }

        public void Remove(params string[] keys)
        {
            foreach (var key in keys)
            {
                var cacheItem = new CacheItem(key);
                if (LookupOne.ContainsKey(cacheItem))
                    LookupOne.Remove(cacheItem);

                if (LookupTwo.ContainsKey(cacheItem))
                    LookupTwo.Remove(cacheItem);
            }
        }
    }
}