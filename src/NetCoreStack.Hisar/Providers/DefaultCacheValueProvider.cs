using System;

namespace NetCoreStack.Hisar
{
    internal class DefaultCacheValueProvider : ICacheValueProvider
    {
        public object TryGetValue<TModel>(object id, DateTimeOffset? absoluteExpiration = default(DateTimeOffset?))
        {
            return default(TModel);
        }
    }
}
