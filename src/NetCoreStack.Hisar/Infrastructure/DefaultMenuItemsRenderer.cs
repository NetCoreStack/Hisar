using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;

namespace NetCoreStack.Hisar
{
    public class DefaultMenuItemsRenderer : IMenuItemsRenderer
    {
        private readonly IEnumerable<IMenuItemsBuilder> _builders;
        public DefaultMenuItemsRenderer(IEnumerable<IMenuItemsBuilder> builders)
        {
            _builders = builders;
        }

        public virtual IDictionary<ComponentPair, IEnumerable<IMenuItem>> PopulateMenuItems(IUrlHelper urlHelper)
        {
            var dictionary = new Dictionary<ComponentPair, IEnumerable<IMenuItem>>();
            foreach (var builder in _builders)
            {
                var componentPair = new ComponentPair(builder.Component.ComponentId, builder.Component.Title, builder.Component.ComponentType);
                if (dictionary.ContainsKey(componentPair))
                    dictionary[componentPair] = builder.Build(urlHelper);
                else
                    dictionary.Add(componentPair, builder.Build(urlHelper));
            }

            return dictionary;
        }

        public virtual IHtmlContent Render(IUrlHelper urlHelper, Action<IDictionary<ComponentPair, IEnumerable<IMenuItem>>> filter = null)
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

                    var anchorTag = new TagBuilder("a");
                    anchorTag.Attributes.Add("href", "#");
                    anchorTag.InnerHtml.Append(title);

                    var spanTag = new TagBuilder("span");
                    spanTag.Attributes.Add("class", "fa fa-chevron-down");
                    anchorTag.InnerHtml.AppendHtml(spanTag);
                    parentTag.InnerHtml.AppendHtml(anchorTag);

                    if (entry.Value != null && entry.Value.Any())
                    {
                        var childMenu = new TagBuilder("ul");
                        childMenu.Attributes.Add("class", "nav child_menu");
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
