using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetCoreStack.Hisar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;

namespace Shared.Library
{
    public class SharedMenuItemsRenderer : DefaultMenuItemsRenderer
    {
        public SharedMenuItemsRenderer(IEnumerable<IMenuItemsBuilder> builders) 
            : base(builders)
        {
        }

        public override IHtmlContent Render(IUrlHelper urlHelper, Action<IDictionary<ComponentPair, IEnumerable<IMenuItem>>> filter = null)
        {
            IDictionary<ComponentPair, IEnumerable<IMenuItem>> menuItems = PopulateMenuItems(urlHelper);
            filter?.Invoke(menuItems);

            using (var writer = new StringWriter())
            {
                foreach (KeyValuePair<ComponentPair, IEnumerable<IMenuItem>> entry in menuItems)
                {
                    ComponentType componentType = entry.Key.ComponentType;
                    string title = entry.Key.Title;

                    var parentTag = new TagBuilder("li");
                    parentTag.Attributes.Add("class", "nav-item");

                    var anchorTag = new TagBuilder("a");
                    anchorTag.Attributes.Add("href", "#");
                    anchorTag.Attributes.Add("data-toggle", "dropdown");
                    anchorTag.Attributes.Add("class", "nav-link");
                    anchorTag.InnerHtml.Append(title);

                    var spanTag = new TagBuilder("span");
                    spanTag.Attributes.Add("class", "fa fa-chevron-down");
                    anchorTag.InnerHtml.AppendHtml(spanTag);
                    parentTag.InnerHtml.AppendHtml(anchorTag);

                    if (entry.Value != null && entry.Value.Any())
                    {
                        var childMenu = new TagBuilder("ul");
                        childMenu.Attributes.Add("class", "nav child_menu dropdown-menu");
                        foreach (var menu in entry.Value)
                        {
                            if (!menu.ShowInMenu)
                                continue;

                            var childLi = new TagBuilder("li");
                            var childAnchor = new TagBuilder("a");

                            childAnchor.Attributes.Add("href", menu.Path);
                            childAnchor.InnerHtml.Append(menu.Text);
                            childLi.InnerHtml.AppendHtml(childAnchor);
                            childMenu.InnerHtml.AppendHtml(childLi);
                        }

                        parentTag.InnerHtml.AppendHtml(childMenu);
                    }

                    parentTag.WriteTo(writer, HtmlEncoder.Default);
                }

                return new HtmlString(writer.ToString());
            }
        }
    }
}
