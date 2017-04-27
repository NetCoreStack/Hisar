// Hisar Cli auto generated component class!
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using NetCoreStack.Hisar;
using System.Reflection;

namespace Hisar.Component.Template
{
    public static partial class ComponentHelper
    {
        public static string ComponentId { get; }

        static ComponentHelper()
        {
            ComponentId = typeof(ComponentHelper).GetTypeInfo().Assembly.GetComponentId();
        }

        public static string Content(IUrlHelper urlHelper, string contentPath)
        {
#if !RELEASE
            var componentHelper = ComponentHelperBase.GetComponentHelper(urlHelper.ActionContext);
            if (componentHelper != null)
            {
                if (componentHelper.IsExternalComponent)
                {
                    return urlHelper.Content(contentPath);
                }
            }
#endif

            return ComponentHelperBase.ResolveContentPath(urlHelper, ComponentId, contentPath);
        }

#pragma warning disable 0162
        public static string ResolveName(string name)
        {
#if !RELEASE
            return name;
#endif

            return $"{ComponentId}.{name}";
        }

        public static string ResolveName<TComponent>()
        {
#if !RELEASE
            return ViewComponentConventions.GetComponentName(typeof(TComponent).GetTypeInfo());
#endif

            var componentName = ViewComponentConventions.GetComponentName(typeof(TComponent).GetTypeInfo());
            return $"{ComponentId}.{componentName}";
        }
    }
}