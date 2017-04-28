// Hisar Cli auto generated component class!
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using NetCoreStack.Hisar;
using System.Reflection;

namespace Hisar.Component.Guideline
{
    public static partial class ComponentHelper
    {
        public static string ComponentId { get; }
        private static readonly string _assemblyName;

        static ComponentHelper()
        {
            var typeInfo = typeof(ComponentHelper).GetTypeInfo();
            ComponentId = typeInfo.Assembly.GetComponentId();
            _assemblyName = typeInfo.Assembly.GetName().Name;
        }

        public static string ComponentContent(this IUrlHelper urlHelper, string contentPath)
        {
            if (ComponentHelperBase.IsExternalComponent(urlHelper.ActionContext))
            {
                return urlHelper.Content(contentPath);
            }

            return ComponentHelperBase.ResolveContentPath(urlHelper, ComponentId, contentPath);
        }
        
        public static string ResolveName(this ViewContext context, string name)
        {
            if (ComponentHelperBase.IsExternalComponent(context))
            {
                return name;
            }

            return $"{_assemblyName}.{name}";
        }

        public static string ResolveName<TComponent>(this ViewContext context)
        {
            if (ComponentHelperBase.IsExternalComponent(context))
            {
                return ViewComponentConventions.GetComponentName(typeof(TComponent).GetTypeInfo());
            }

            var componentName = ViewComponentConventions.GetComponentName(typeof(TComponent).GetTypeInfo());
            return $"{_assemblyName}.{componentName}";
        }
    }
}