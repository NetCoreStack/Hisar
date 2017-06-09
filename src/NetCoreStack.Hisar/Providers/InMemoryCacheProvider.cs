using Microsoft.AspNetCore.Mvc;
using NetCoreStack.Contracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreStack.Hisar
{
    public class InMemoryCacheProvider : IMemoryCacheProvider
    {
        private static object _lockObject = new object();

        private bool _disposed;

        private readonly ConcurrentDictionary<CacheItem, IModelKey<string>> LookupOne 
            = new ConcurrentDictionary<CacheItem, IModelKey<string>>();

        private readonly ConcurrentDictionary<CacheItem, IModelKey<long>> LookupTwo 
            = new ConcurrentDictionary<CacheItem, IModelKey<long>>();

        public IModelKeyGenerator ModelKeyGenerator { get; }

        public InMemoryCacheProvider( IModelKeyGenerator keyGenerator)
        {
            ModelKeyGenerator = keyGenerator;
        }

        private TModel TryGetValue<TModel, TKey>(ActionContext context, TKey id, CacheItem key) where TModel : IModelKey<TKey>
        {
            var valueProviderType = typeof(ICacheValueProvider<TModel>);
            var instance = (ICacheValueProvider<TModel>)context.HttpContext.RequestServices.GetService(valueProviderType);
            if (instance != null)
            {
                lock (_lockObject)
                {
                    // cache was empty before we got the lock, check again inside the lock
                    if (typeof(TKey) == typeof(String))
                    {
                        if (LookupTwo.TryGetValue(key, out IModelKey<long> item))
                            return (TModel)item;
                    }

                    if (typeof(TKey).IsNumericType())
                    {
                        if (LookupOne.TryGetValue(key, out IModelKey<string> item))
                            return (TModel)item;
                    }

                    var value = instance.TryGetValue(context, id, key);
                    if (value != null)
                    {
                        return Set<TModel>(key, value);
                    }
                }
            }

            return default(TModel);
        }

        private bool IsExpiredInLookupOne(CacheItem cacheItem)
        {
            cacheItem = LookupOne.Keys.FirstOrDefault(x => x == cacheItem);
            if (cacheItem.AbsoluteExpiration.HasValue)
            {
                if (cacheItem.AbsoluteExpiration.Value < DateTime.Now)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsExpiredInLookupTwo(CacheItem cacheItem)
        {
            cacheItem = LookupTwo.Keys.FirstOrDefault(x => x == cacheItem);
            if (cacheItem.AbsoluteExpiration.HasValue)
            {
                if (cacheItem.AbsoluteExpiration.Value < DateTime.Now)
                {
                    return true;
                }
            }

            return false;
        }

        public bool TryGetValue(CacheItem key, out IModelKey<string> value)
        {
            value = null;
            var found = false;

            if (LookupOne.TryGetValue(key, out IModelKey<string> item))
            {
                if (IsExpiredInLookupOne(key))
                {
                    LookupOne.TryRemove(key, out IModelKey<string> item2);
                    return found;
                }

                value = item;
                found = true;
            }

            return found;
        }

        public bool TryGetValue(CacheItem key, out IModelKey<long> value)
        {
            value = null;
            var found = false;

            if (LookupTwo.TryGetValue(key, out IModelKey<long> item))
            {
                if (IsExpiredInLookupTwo(key))
                {
                    LookupTwo.TryRemove(key, out IModelKey<long> item2);
                    return found;
                }

                value = item;
                found = true;
            }

            return found;
        }

        public TModel Set<TModel>(CacheItem key, TModel value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                return default(TModel);
            }

            if (value is IModelKey<string>)
            {
                if (LookupOne.TryGetValue(key, out IModelKey<string> item))
                {
                    LookupOne.TryUpdate(key, (IModelKey<string>)value, item);
                }
                else
                {
                    LookupOne.TryAdd(key, (IModelKey<string>)value);
                }
                
                return value;
            }

            if (value is IModelKey<long>)
            {
                if (LookupTwo.TryGetValue(key, out IModelKey<long> item))
                {
                    LookupTwo.TryUpdate(key, (IModelKey<long>)value, item);
                }
                else
                {
                    LookupTwo.TryAdd(key, (IModelKey<long>)value);
                }

                return value;
            }

            throw new NotSupportedException(nameof(TModel));
        }

        public TModel GetOrCreate<TModel>(ActionContext context, long id) where TModel : IModelKey<long>
        {
            var cacheItem = ModelKeyGenerator.CreateCacheItem<TModel>(id);
            if (LookupTwo.TryGetValue(cacheItem, out IModelKey<long> item))
            {
                if (IsExpiredInLookupTwo(cacheItem))
                {
                    lock (_lockObject)
                    {
                        // cache item was expire before we got the lock, check again inside the lock
                        if (IsExpiredInLookupTwo(cacheItem))
                        {
                            return TryGetValue<TModel, long>(context, id, cacheItem);
                        }

                        return (TModel)item;
                    }
                }

                return (TModel)item;
            }

            // value setter lock
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
            var cacheItem = ModelKeyGenerator.CreateCacheItem<TModel>(id);
            if (LookupOne.TryGetValue(cacheItem, out IModelKey<string> item))
            {
                if (IsExpiredInLookupOne(cacheItem))
                {
                    lock (_lockObject)
                    {
                        // cache item was expire before we got the lock, check again inside the lock
                        if (IsExpiredInLookupOne(cacheItem))
                        {
                            return TryGetValue<TModel, string>(context, id, cacheItem);
                        }

                        return (TModel)item;
                    }
                }

                return (TModel)item;
            }

            // value setter lock
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
            var cacheItem = ModelKeyGenerator.CreateCacheItem<TModel>(id);
            if (LookupOne.ContainsKey(cacheItem))
                LookupOne.TryRemove(cacheItem, out IModelKey<string> value);
        }

        public void Remove<TModel>(long id) where TModel : IModelKey<long>
        {
            var cacheItem = ModelKeyGenerator.CreateCacheItem<TModel>(id);
            if (LookupTwo.ContainsKey(cacheItem))
                LookupTwo.TryRemove(cacheItem, out IModelKey<long> value);
        }

        public void Remove(params string[] keys)
        {
            foreach (var key in keys)
            {
                var cacheItem = new CacheItem(key);
                if (LookupOne.ContainsKey(cacheItem))
                    LookupOne.TryRemove(cacheItem, out IModelKey<string> value);

                if (LookupTwo.ContainsKey(cacheItem))
                    LookupTwo.TryRemove(cacheItem, out IModelKey<long> value);
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    GC.SuppressFinalize(this);
                }

                _disposed = true;
            }
        }

        private void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(typeof(InMemoryCacheProvider).FullName);
            }
        }
    }
}