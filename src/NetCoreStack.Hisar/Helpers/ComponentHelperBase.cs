using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace NetCoreStack.Hisar
{
    public class ComponentHelperBase
    {
        private static string _prefixFormat = "~/{0}/";

        public static RunningComponentHelper GetComponentHelper(ActionContext context)
        {
            return ServiceProviderServiceExtensions.GetService<RunningComponentHelper>(context.HttpContext.RequestServices);
        }

        public static string ResolveContentPath(IUrlHelper urlHelper, string componentId, string contentPath)
        {
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

            contentPath = contentPath.Insert(0, string.Format(_prefixFormat, componentId));
            return urlHelper.Content(contentPath);
        }
    }
}
