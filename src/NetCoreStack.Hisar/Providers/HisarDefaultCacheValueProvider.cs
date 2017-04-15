using System;

namespace NetCoreStack.Hisar
{
    public class HisarDefaultCacheValueProvider : IHisarCacheValueProvider
    {
        public object GetValueSetter(string cacheName, ref DateTimeOffset? absoluteExpiration)
        {
            return null;
        }
    }
}
