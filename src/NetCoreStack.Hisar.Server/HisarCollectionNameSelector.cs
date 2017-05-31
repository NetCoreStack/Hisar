using NetCoreStack.Data;
using NetCoreStack.Mvc.Types;
using System;
using System.Reflection;

namespace NetCoreStack.Hisar.Server
{
    public class HisarCollectionNameSelector : DefaultCollectionNameSelector
    {
        const string CollectionNameFormat = "{0}.{1}";

        public override string GetCollectionName<TEntity>()
        {
            var entityTypeInfo = typeof(TEntity).GetTypeInfo();
            var componentId = entityTypeInfo.Assembly.GetComponentId();

            string name;
            if (entityTypeInfo.BaseType.Equals(typeof(object)))
            {
                name = GetCollectioNameFromInterface<TEntity>();
            }
            else
            {
                name = GetCollectionNameFromType(typeof(TEntity));
            }

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Collection name could not be found!");

            if (entityTypeInfo == typeof(SystemLog).GetTypeInfo())
            {
                return string.Format(CollectionNameFormat, "Hisar", name);
            }

            return string.Format(CollectionNameFormat, componentId, name);
        }
    }
}
