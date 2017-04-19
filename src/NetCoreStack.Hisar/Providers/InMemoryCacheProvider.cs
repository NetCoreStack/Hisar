using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _memoryCache;
        private readonly IServiceProvider _serviceProvider;

        public InMemoryCacheProvider(IMemoryCache memoryCache, IServiceProvider serviceProvider)
        {
            _memoryCache = memoryCache;
            _serviceProvider = serviceProvider;
        }

        public List<TEntity> GetList<TEntity>()
        {
            var cacheName = typeof(TEntity).Name;
            var value = _memoryCache.Get(typeof(TEntity).Name);
            if (value == null)
            {
                lock (_lockObject)
                {
                    // cache was empty before we got the lock, check again inside the lock
                    if (_memoryCache.TryGetValue(cacheName, out object lockedValue))
                        return (List<TEntity>)lockedValue;

                    lockedValue = _memoryCache.Get(typeof(TEntity).Name);

                    // cache is still empty, so retreive the value here
                    if (lockedValue == null)
                    {
                        // store the value in the cache here
                        var valueProviders = _serviceProvider.GetServices<IHisarCacheValueProvider>();
                        DateTimeOffset? absoluteExpiration = null;
                        foreach (var valueProvider in valueProviders)
                        {
                            lockedValue = valueProvider.GetValueSetter(cacheName, ref absoluteExpiration);
                            if (lockedValue != null)
                            {
                                value = lockedValue;

                                if (absoluteExpiration.HasValue)
                                    _memoryCache.Set(cacheName, value, absoluteExpiration.Value);
                                else
                                    _memoryCache.Set(cacheName, value);

                                break;
                            }
                        }
                    }
                }
            }

            return (List<TEntity>)value;
        }

        public object SetObject(string key, object value, CacheProviderOptions options)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException("Cache value is null");
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var entryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = options.AbsoluteExpiration,
                Priority = options.Priority
            };

            _memoryCache.Set(key, value, entryOptions);
            return value;
        }

        public object GetObject(string key)
        {
            return _memoryCache.Get(key);
        }

        public T GetObject<T>(string key)
        {
            return (T)_memoryCache.Get(key);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        //public TEntity GetItem<TEntity>(long? id) where TEntity : EntityBase
        //{
        //    if (!id.HasValue)
        //        return null;

        //    return GetItem((TEntity e) => e.ID == id);
        //}

        public TEntity GetItem<TEntity>(Func<TEntity, bool> idSelector)
        {
            var list = GetList<TEntity>();
            return list.SingleOrDefault(idSelector);
        }
    }
}