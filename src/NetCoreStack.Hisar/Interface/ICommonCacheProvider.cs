using NetCoreStack.Contracts;
using NetCoreStack.Mvc.Types;
using System.Collections.Generic;

namespace NetCoreStack.Hisar
{
    public interface ICommonCacheProvider
    {
        void SetItem<TModel, TKey>(TModel value, CacheProviderOptions options) where TModel : IModelKey<TKey>;

        void Remove(params string[] keys);

        void Remove<TModel>(string id) where TModel : IModelKey<string>;

        void Remove<TModel>(long id) where TModel : IModelKey<long>;

        IEnumerable<TModel> GetList<TModel>() where TModel : IModelKey<string>;

        TModel GetOrCreate<TModel>(long id) where TModel : IModelKey<long>;

        TModel GetOrCreate<TModel>(string id) where TModel : IModelKey<string>;

        TModel GetItem<TModel>(string id) where TModel : IModelKey<string>;

        TModel GetItem<TModel>(long id) where TModel : IModelKey<long>;
    }
}
