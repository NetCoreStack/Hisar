// Hisar Cli auto generated component info class!
using System.Reflection;
using NetCoreStack.Hisar;
using Microsoft.AspNetCore.Mvc;

namespace Hisar.Component.Guideline
{
    public static class GuidelineComponent
    {
        public static string ComponentId { get; }

        static GuidelineComponent()
        {
            typeof(GuidelineComponent).GetTypeInfo().Assembly.GetComponentId();
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
            return urlHelper.Content(contentPath);
        }
    }
}