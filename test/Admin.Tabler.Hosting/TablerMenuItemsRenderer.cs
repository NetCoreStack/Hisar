using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using NetCoreStack.Hisar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Admin.Tabler.Hosting
{
    public class TablerMenuItemsRenderer : DefaultMenuItemsRenderer
    {
        private readonly IViewRenderService _viewRenderService;


        public TablerMenuItemsRenderer(IEnumerable<IMenuItemsBuilder> builders, IViewRenderService viewRenderService) : base(builders)
        {
            _viewRenderService = viewRenderService;
        }

        public override IHtmlContent Render(IUrlHelper urlHelper, Action<IDictionary<ComponentPair, IEnumerable<IMenuItem>>> filter = null)
        {
            IDictionary<ComponentPair, IEnumerable<IMenuItem>> menuItems = PopulateMenuItems(urlHelper);
            filter?.Invoke(menuItems);

            var partialView = "MenuItemsPv";

            var result = _viewRenderService.RenderToStringAsync(partialView, menuItems).GetAwaiter().GetResult();

            var contentBuilder = result;
            return new HtmlString(contentBuilder);
        }

        public static string GetString(IHtmlContent content)
        {
            using (var writer = new StringWriter())
            {

                content.WriteTo(writer, HtmlEncoder.Default);
                return writer.ToString();
            }

        }
    }
 
}
