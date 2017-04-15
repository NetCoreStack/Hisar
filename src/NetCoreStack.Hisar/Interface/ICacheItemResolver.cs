using System.Collections.Generic;

namespace NetCoreStack.Hisar
{
    public interface ICacheItemResolver
    {
        IList<HisarCacheAttribute> CacheItems { get; }

        HisarCacheAttribute Resolve(string cacheName);
    }
}
