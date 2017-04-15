using System;
using System.Collections.Generic;
using System.Reflection;

namespace NetCoreStack.Hisar
{
    public static class HisarTypeExtensions
    {
        public static HisarCacheAttribute GetCacheAttribute(this Type type)
        {
            var attr = type.GetTypeInfo().GetCustomAttribute<HisarCacheAttribute>();
            return attr;
        }

        public static IEnumerable<TAttribute> GetTypesAttributes<TAttribute>(this Assembly assembly) where TAttribute : Attribute
        {
            List<TAttribute> attrs = new List<TAttribute>();
            foreach (Type type in assembly.GetTypes())
                attrs.AddRange(type.GetTypeInfo().GetCustomAttributes<TAttribute>());
            return attrs;
        }
    }
}
