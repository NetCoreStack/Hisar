using System.Collections.Generic;
using System.Linq;

namespace NetCoreStack.Hisar
{
    public class DefaultCacheItemResolver : ICacheItemResolver
    {
        public IList<HisarCacheAttribute> CacheItems { get; }

        public DefaultCacheItemResolver(IList<HisarCacheAttribute> cacheItems)
        {
            CacheItems = cacheItems;
        }

        public HisarCacheAttribute Resolve(string cacheName)
        {
            return CacheItems.FirstOrDefault(x => x.Key == cacheName);
        }
    }
}