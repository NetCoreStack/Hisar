namespace NetCoreStack.Hisar
{
    public interface ICacheValueProvider
    {
        object TryGetValue<TModel>(object id, CacheItem key);
    }
}