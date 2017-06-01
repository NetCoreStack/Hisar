using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCoreStack.Hisar
{
    public static class HtmlHelperExtensions
    {
        public static IMenuItemsRenderer GetMenuItemsRenderer(ViewContext context)
        {
            return ServiceProviderServiceExtensions.GetService<IMenuItemsRenderer>(context.HttpContext.RequestServices);
        }

        public static IUrlHelper GetUrlHelper(ViewContext context)
        {
            var factory = ServiceProviderServiceExtensions.GetService<IUrlHelperFactory>(context.HttpContext.RequestServices);
            return factory.GetUrlHelper(context);
        }

        public static IHtmlContent RenderMenu(this IHtmlHelper htmlHelper)
        {
            var renderer = GetMenuItemsRenderer(htmlHelper.ViewContext);
            var urlHelper = GetUrlHelper(htmlHelper.ViewContext);
            return renderer.Render(urlHelper);
        }

        public static async Task RenderComponentsScriptsAsync(this IHtmlHelper htmlHelper)
        {
            if (htmlHelper.ViewContext.ViewData.TryGetValue(nameof(ComponentScriptsTagHelper), out object items))
            {
                if (items is List<string> scripts)
                {
                    await htmlHelper.ViewContext.Writer.WriteAsync(string.Join(string.Empty, scripts));
                }
            }
        }
    }
}
