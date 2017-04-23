// Hisar Cli auto generated component info class!
using System.Reflection;
using NetCoreStack.Hisar;
using Microsoft.AspNetCore.Mvc;

namespace Hisar.Component.Carousel
{
    public static class ComponentHelper
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
    }
}