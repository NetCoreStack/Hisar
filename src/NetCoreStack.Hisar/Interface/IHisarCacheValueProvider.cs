using System;

namespace NetCoreStack.Hisar
{
    public interface IHisarCacheValueProvider
    {
        object GetValueSetter(string cacheName, ref DateTimeOffset? absoluteExpiration);
    }
}