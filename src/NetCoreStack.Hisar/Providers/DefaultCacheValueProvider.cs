using System;

namespace NetCoreStack.Hisar
{
    internal class DefaultCacheValueProvider : ICacheValueProvider
    {
        public object TryGetValue<TModel>(object id, CacheItem key)
        {
            return default(TModel);
        }
    }
}
