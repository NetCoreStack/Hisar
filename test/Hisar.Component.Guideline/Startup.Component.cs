// Hisar Cli auto generated component info class!
using System.Reflection;
using NetCoreStack.Hisar;
using Microsoft.AspNetCore.Mvc;

namespace Hisar.Component.Guideline
{
    public static class ComponentInfo
    {
        public static string ComponentId { get; }

        static ComponentInfo()
        {
            typeof(ComponentInfo).GetTypeInfo().Assembly.GetComponentId();
        }

        public static string ComponentContent(this IUrlHelper urlHelper, string contentPath)
        {
#if !RELEASE
            var componentHelper = ComponentInfoHelper.GetComponentHelper(urlHelper.ActionContext);
            if (componentHelper != null)
            {
                if (componentHelper.IsExternalComponent)
                {
                    return urlHelper.Content(contentPath);
                }
            }
#endif
            return urlHelper.Content(contentPath);
        }
    }
}