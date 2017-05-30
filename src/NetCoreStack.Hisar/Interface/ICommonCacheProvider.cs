using Microsoft.AspNetCore.Mvc;
using NetCoreStack.Contracts;

namespace NetCoreStack.Hisar
{
    public interface ICommonCacheProvider
    {
        void Remove(params string[] keys);

        void Remove<TModel>(string id) where TModel : IModelKey<string>;

        void Remove<TModel>(long id) where TModel : IModelKey<long>;

        TModel GetOrCreate<TModel>(ActionContext context, long id) where TModel : IModelKey<long>;

        TModel GetOrCreate<TModel>(ActionContext context, string id) where TModel : IModelKey<string>;
    }
}
