using NetCoreStack.Hisar;
using System;

namespace Hosting
{
    public class WebClientCacheValueProvider : IHisarCacheValueProvider
    {
        public object GetValueSetter(string cacheName, ref DateTimeOffset? absoluteExpiration)
        {
            return null;
        }
    }
}
