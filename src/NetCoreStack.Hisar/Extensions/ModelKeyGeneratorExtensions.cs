using NetCoreStack.Contracts;

namespace NetCoreStack.Hisar
{
    public static class ModelKeyGeneratorExtensions
    {
        public static CacheItem CreateCacheItem<TModel>(this IModelKeyGenerator keyGenerator, string id) where TModel : IModelKey<string>
        {
            return new CacheItem(keyGenerator.CreateKey<TModel>(id));
        }

        public static CacheItem CreateCacheItem<TModel>(this IModelKeyGenerator keyGenerator, long id) where TModel : IModelKey<long>
        {
            return new CacheItem(keyGenerator.CreateKey<TModel>(id));
        }
    }
}
