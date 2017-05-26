using Microsoft.Extensions.DependencyInjection;
using NetCoreStack.Contracts;
using NetCoreStack.Mvc.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreStack.Hisar
{
    public class InMemoryCacheProvider : ICommonCacheProvider
    {
        private static object _lockObject = new object();

        private readonly IDictionary<string, IModelKey<string>> LookupOne 
            = new Dictionary<string, IModelKey<string>>();

        private readonly IDictionary<string, IModelKey<long>> LookupTwo 
            = new Dictionary<string, IModelKey<long>>();

        private readonly IDictionary<string, object> Lookup = new Dictionary<string, object>();

        private readonly IModelKeyGenerator _keyGenerator;
        private readonly IServiceProvider _serviceProvider;

        public InMemoryCacheProvider(IModelKeyGenerator keyGenerator, 
            IServiceProvider serviceProvider)
        {
            _keyGenerator = keyGenerator;
            _serviceProvider = serviceProvider;
        }
        
        private TModel TryGetValue<TModel, TKey>(TKey id, string key) where TModel : IModelKey<TKey>
        {
            var valueProviders = _serviceProvider.GetServices<ICacheValueProvider>();
            if (valueProviders != null)
            {
                foreach (var provider in valueProviders)
                {
                    var instance = provider.TryGetValue<TModel>(id);
                    if (instance != null)
                    {
                        if (instance is IModelKey<string>)
                        {
                            LookupOne.Add(key, (IModelKey<string>)instance);
                            return (TModel)instance;
                        }

                        if (instance is IModelKey<long>)
                        {
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
            List<string> keys = new List<string>();
            keys = LookupOne.Keys.Where(x => x.StartsWith(itemKey)).ToList();
            var values = keys.Select(x => (TModel)LookupOne[x]).ToList();
            return values;
        }

        public TModel GetItem<TModel>(string id) where TModel : IModelKey<string>
        {
            var itemKey = _keyGenerator.CreateKey<TModel>(id);
            if (LookupOne.TryGetValue(itemKey, out IModelKey<string> item))
                return (TModel)item;

            return default(TModel);
        }

        public TModel GetItem<TModel>(long id) where TModel : IModelKey<long>
        {
            var itemKey = _keyGenerator.CreateKey<TModel>(id);
            if (LookupTwo.TryGetValue(itemKey, out IModelKey<long> item))
                return (TModel)item;

            return default(TModel);
        }

        public TModel GetOrCreate<TModel>(long id) where TModel : IModelKey<long>
        {
            var itemKey = _keyGenerator.CreateKey<TModel>(id);
            if (LookupTwo.TryGetValue(itemKey, out IModelKey<long> item))
                return (TModel)item;

            lock (_lockObject)
            {
                // cache was empty before we got the lock, check again inside the lock
                if (LookupTwo.TryGetValue(itemKey, out IModelKey<long> item2))
                    return (TModel)item2;

                return TryGetValue<TModel, long>(id, itemKey);
            }
        }

        public TModel GetOrCreate<TModel>(string id) where TModel : IModelKey<string>
        {
            var itemKey = _keyGenerator.CreateKey<TModel>(id);
            if (LookupOne.TryGetValue(itemKey, out IModelKey<string> item))
                return (TModel)item;

            lock (_lockObject)
            {
                // cache was empty before we got the lock, check again inside the lock
                if (LookupOne.TryGetValue(itemKey, out IModelKey<string> item2))
                    return (TModel)item2;

                return TryGetValue<TModel, string>(id, itemKey);
            }
        }

        public void SetItem<TModel, TKey>(TModel value, CacheProviderOptions options) where TModel : IModelKey<TKey>
        {
            if (value == null)
                return;

            if (typeof(TModel) is IModelKey<string>)
            {
                var item = (IModelKey<string>)value;
                var itemKey = _keyGenerator.CreateKey<IModelKey<string>>(item.Id);
                if (LookupOne.ContainsKey(itemKey))
                    LookupOne[itemKey] = item;
                else
                    LookupOne.Add(itemKey, item);
            }
            else if (typeof(TModel) is IModelKey<long>)
            {
                var item = (IModelKey<long>)value;
                var itemKey = _keyGenerator.CreateKey<IModelKey<long>>(item.Id);
                if (LookupTwo.ContainsKey(itemKey))
                    LookupTwo[itemKey] = item;
                else
                    LookupTwo.Add(itemKey, item);
            }

            throw new NotSupportedException(nameof(TModel));
        }

        public void Remove<TModel>(string id) where TModel : IModelKey<string>
        {
            var itemKey = _keyGenerator.CreateKey<TModel>(id);
            if (LookupOne.ContainsKey(itemKey))
                LookupOne.Remove(itemKey);
        }

        public void Remove<TModel>(long id) where TModel : IModelKey<long>
        {
            var itemKey = _keyGenerator.CreateKey<TModel>(id);
            if (LookupTwo.ContainsKey(itemKey))
                LookupTwo.Remove(itemKey);
        }

        public void Remove(params string[] keys)
        {
            foreach (var key in keys)
            {
                if (LookupOne.ContainsKey(key))
                    LookupOne.Remove(key);

                if (LookupTwo.ContainsKey(key))
                    LookupTwo.Remove(key);
            }
        }
    }
}