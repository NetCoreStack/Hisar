using NetCoreStack.Contracts;

namespace NetCoreStack.Hisar
{
    public interface IModelKeyGenerator
    {
        string CreateKey<TModel>(string id) where TModel : IModelKey<string>;

        string CreateKey<TModel>(long id) where TModel : IModelKey<long>;

        string CreateKey<TModel>();
    }
}
