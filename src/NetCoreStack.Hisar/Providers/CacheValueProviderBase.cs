using Microsoft.AspNetCore.Mvc;

namespace NetCoreStack.Hisar
{
    public abstract class CacheValueProviderBase<TModel> : ICacheValueProvider<TModel>
    {
        public abstract TModel TryGetValue(ActionContext context, object id, CacheItem key);
    }
}