using NetCoreStack.Contracts;

namespace NetCoreStack.Hisar
{
    public interface IModelKeyGenerator
    {
        string Create<TModel>(object id);

        string CreateKey<TModel>(string id) where TModel : IModelKey<string>;

        string CreateKey<TModel>(long id) where TModel : IModelKey<long>;

        string CreateKey<TModel>();
    }
}
