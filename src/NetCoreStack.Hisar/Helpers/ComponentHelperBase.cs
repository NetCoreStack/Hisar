using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace NetCoreStack.Hisar
{
    public static class ComponentHelperBase
    {
        private static string _prefixFormat = "~/{0}/";

        public static RunningComponentHelper GetComponentHelper(ActionContext context)
        {
            return ServiceProviderServiceExtensions.GetService<RunningComponentHelper>(context.HttpContext.RequestServices);
        }

        public static bool IsExternalComponent(ActionContext context)
        {
            var componentHelper = GetComponentHelper(context);
            if (componentHelper != null)
            {
                return componentHelper.IsExternalComponent;
            }

            return false;
        }

        public static string ResolveContentPath(IUrlHelper urlHelper, string componentId, string contentPath)
        {
            if (IsExternalComponent(urlHelper.ActionContext))
            {
                return urlHelper.Content(contentPath);
            }

            if (string.IsNullOrEmpty(contentPath))
            {
                throw new ArgumentNullException(nameof(contentPath));
            }

            if (contentPath.StartsWith("/"))
            {
                contentPath = contentPath.Substring(1);
            }
            else if (contentPath.StartsWith("~/"))
            {
                contentPath = contentPath.Substring(2);
            }

            contentPath = contentPath.Insert(0, string.Format(_prefixFormat, componentId.ToLowerInvariant()));
            return urlHelper.Content(contentPath);
        }

        public static string ResolveViewComponentName(ViewContext context, string assemblyName, string name)
        {
            if (IsExternalComponent(context))
            {
                return name;
            }

            return $"{assemblyName}.{name}";
        }

        public static string ResolveViewComponentName<TComponent>(ViewContext context, string assemblyName)
        {
            if (IsExternalComponent(context))
            {
                return ViewComponentConventions.GetComponentName(typeof(TComponent).GetTypeInfo());
            }

            var componentName = ViewComponentConventions.GetComponentName(typeof(TComponent).GetTypeInfo());
            return $"{assemblyName}.{componentName}";
        }
    }
}
