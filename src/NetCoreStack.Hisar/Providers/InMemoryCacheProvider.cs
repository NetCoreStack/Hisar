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
        private readonly IServiceProvider _serviceProvider;

        public InMemoryCacheProvider(IModelKeyGenerator keyGenerator, 
            IServiceProvider serviceProvider)
        {
            _keyGenerator = keyGenerator;
            _serviceProvider = serviceProvider;
        }
        
        private TModel TryGetValue<TModel, TKey>(TKey id, CacheItem key) where TModel : IModelKey<TKey>
        {
            var valueProviders = _serviceProvider.GetServices<ICacheValueProvider>();
            if (valueProviders != null)
            {
                foreach (var provider in valueProviders)
                {
                    var instance = provider.TryGetValue<TModel>(id, key);
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

        public IEnumerable<TModel> GetList<TModel>() where TModel : IModelKey<string>
        {
            var itemKey = _keyGenerator.CreateKey<TModel>();
            var cacheItem = new CacheItem(itemKey);
            List<CacheItem> lookupKeys = new List<CacheItem>();
            lookupKeys = LookupOne.Keys.Where(x => x.Key.StartsWith(itemKey)).ToList();
            var values = lookupKeys.Select(x => (TModel)LookupOne[x]).ToList();
            return values;
        }

        public TModel GetItem<TModel>(string id) where TModel : IModelKey<string>
        {
            var cacheItem = _keyGenerator.CreateCacheItem<TModel>(id);
            if (LookupOne.TryGetValue(cacheItem, out IModelKey<string> item))
                return (TModel)item;

            return default(TModel);
        }

        public TModel GetItem<TModel>(long id) where TModel : IModelKey<long>
        {
            var cacheItem = _keyGenerator.CreateCacheItem<TModel>(id);
            if (LookupTwo.TryGetValue(cacheItem, out IModelKey<long> item))
                return (TModel)item;

            return default(TModel);
        }

        public TModel GetOrCreate<TModel>(long id) where TModel : IModelKey<long>
        {
            var cacheItem = _keyGenerator.CreateCacheItem<TModel>(id);
            if (LookupTwo.TryGetValue(cacheItem, out IModelKey<long> item))
                return (TModel)item;

            lock (_lockObject)
            {
                // cache was empty before we got the lock, check again inside the lock
                if (LookupTwo.TryGetValue(cacheItem, out IModelKey<long> item2))
                    return (TModel)item2;

                return TryGetValue<TModel, long>(id, cacheItem);
            }
        }

        public TModel GetOrCreate<TModel>(string id) where TModel : IModelKey<string>
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
                            return TryGetValue<TModel, string>(id, cacheItem);
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

                return TryGetValue<TModel, string>(id, cacheItem);
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