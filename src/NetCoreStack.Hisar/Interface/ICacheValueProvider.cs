using Microsoft.AspNetCore.Mvc;

namespace NetCoreStack.Hisar
{
    public interface ICacheValueProvider<TModel>
    {
        TModel TryGetValue(ActionContext context, object id, CacheItem key);
    }
}