using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;

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
    }
}
