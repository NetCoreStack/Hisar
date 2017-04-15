using System;

namespace NetCoreStack.Hisar
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HisarCacheAttribute : Attribute
    {
        public string Key { get; set; }
        public string DisplayName { get; set; }
    }
}
