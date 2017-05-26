using NetCoreStack.Contracts;
using System;
using System.Reflection;

namespace NetCoreStack.Hisar
{
    public class DefaultModelKeyGenerator : IModelKeyGenerator
    {
        const string ItemKeyFormat = "{0}.{1}.{2}"; // ComponentId.TypeName.Id
        const string LookupKeyFormat = "{0}.{1}"; // ComponentId.TypeName

        protected string CreateInternal<TModel>(object id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var entityTypeInfo = typeof(TModel).GetTypeInfo();
            var componentId = entityTypeInfo.Assembly.GetComponentId();
            var name = entityTypeInfo.Name;
            return string.Format(ItemKeyFormat, componentId, name, id);
        }

        public string CreateKey<TModel>(string id) where TModel : IModelKey<string>
        {
            return CreateInternal<TModel>(id);
        }

        public string CreateKey<TModel>(long id) where TModel : IModelKey<long>
        {
            return CreateInternal<TModel>(id);
        }

        public string CreateKey<TModel>()
        {
            var entityTypeInfo = typeof(TModel).GetTypeInfo();
            var componentId = entityTypeInfo.Assembly.GetComponentId();
            var name = entityTypeInfo.Name;
            return string.Format(LookupKeyFormat, componentId, name);
        }
    }
}
