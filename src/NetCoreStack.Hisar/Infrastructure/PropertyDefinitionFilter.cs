using Microsoft.AspNetCore.Mvc;
using NetCoreStack.Mvc;
using System.Reflection;

namespace NetCoreStack.Hisar
{
    internal class PropertyDefinitionFilter : IPropertyDefinitionFilter
    {
        private readonly HisarAssemblyComponentsLoader _componentsLoader;

        public PropertyDefinitionFilter(HisarAssemblyComponentsLoader componentsLoader)
        {
            _componentsLoader = componentsLoader;
        }

        public void Invoke(ActionContext context, PropertyInfo propertyInfo, PropertyDefinition property, bool forViewModel)
        {
            if (!string.IsNullOrEmpty(property.DataSourceUrl))
            {
                var componentId = propertyInfo.DeclaringType.GetTypeInfo().Assembly.GetComponentId();
                if (_componentsLoader.ComponentAssemblyLookup.TryGetValue(componentId, out Assembly assembly))
                {
                    var path = property.DataSourceUrl;
                    if (path.StartsWith("/"))
                    {
                        path = $"/{componentId}{path}";
                    }
                    else
                    {
                        path = $"/{componentId}/{path}";
                    }

                    property.DataSourceUrl = path;
                }
            }
        }
    }
}
