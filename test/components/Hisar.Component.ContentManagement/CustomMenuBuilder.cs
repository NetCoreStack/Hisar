using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NetCoreStack.Hisar;

namespace Hisar.Component.ContentManagement
{
    public class CustomMenuBuilder : DefaultMenuItemsBuilder<Startup>
    {
        public override IEnumerable<IMenuItem> Build(IUrlHelper urlHelper)
        {
            var items = new List<IMenuItem>
            {
                new DefaultMenuItem
                {
                    Icon = "file",
                    Order = 1,
                    Path = ResolvePath(urlHelper, $"~/Home/New"),
                    ShowInMenu = true,
                    Text = "New Content"
                },

                new DefaultMenuItem
                {
                    Icon = "file",
                    Order = 1,
                    Path = ResolvePath(urlHelper, $"~/Home/List"),
                    ShowInMenu = true,
                    Text = "Contents"
                }
            };

            return items;
        }
    }
}
