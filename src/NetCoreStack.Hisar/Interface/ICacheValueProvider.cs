using Microsoft.AspNetCore.Mvc;

namespace NetCoreStack.Hisar
{
    public interface ICacheValueProvider
    {
        object TryGetValue<TModel>(ActionContext context, object id, CacheItem key);
    }
}