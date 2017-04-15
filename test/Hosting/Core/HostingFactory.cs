using NetCoreStack.Hisar;
using System.Collections.Generic;

namespace Hosting
{
    public static class HostingFactory
    {
        public static List<HisarCacheAttribute> CacheItems { get; set; }

        static HostingFactory()
        {
            CacheItems = new List<HisarCacheAttribute>();
        }
    }
}