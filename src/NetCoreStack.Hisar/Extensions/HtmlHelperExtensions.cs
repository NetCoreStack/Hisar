using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static NetCoreStack.Hisar.ComponentScriptsTagHelper;

namespace NetCoreStack.Hisar
{
    public static class HtmlHelperExtensions
    {
        private static readonly string _hisarConnectionFileName = "hisar-connection.html";
        private static readonly string _resourcePath = "NetCoreStack.Hisar.Resources.hisar-connection.html";

        private static string _connectionFileContent;

        public static string ConnectionFileContent
        {
            get
            {
                if (!string.IsNullOrEmpty(_connectionFileContent))
                {
                    return _connectionFileContent;
                }

                using(Stream stream = typeof(HtmlHelperExtensions).Assembly.GetManifestResourceStream(_resourcePath))
                using(StreamReader sr = new StreamReader(stream))
                {
                    _connectionFileContent = sr.ReadToEnd();
                }

                return _connectionFileContent;
            }
        }
            

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
            if (htmlHelper.ViewContext.HttpContext.Items.TryGetValue(ScriptsDictionaryKey, out object items))
            {
                if (items is List<string> scripts)
                {
                    await htmlHelper.ViewContext.Writer.WriteAsync(string.Join(string.Empty, scripts));
                }
            }

            if (WebCliProxyInformation.Instance.EnableLiveReload)
            {
                await htmlHelper.ViewContext.Writer.WriteAsync(ConnectionFileContent);
            }
        }
    }
}
