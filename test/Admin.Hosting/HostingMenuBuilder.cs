using Microsoft.AspNetCore.Mvc;
using NetCoreStack.Hisar;
using System.Collections.Generic;

namespace Admin.Hosting
{
    public class HostingMenuBuilder : DefaultMenuItemsBuilder<Startup>
    {
        public override IEnumerable<IMenuItem> Build(IUrlHelper urlHelper)
        {
            var items = new List<IMenuItem>
            {
                new DefaultMenuItem
                {
                    Icon = "home",
                    Order = 1,
                    Path = ResolvePath(urlHelper, "/"),
                    ShowInMenu = true,
                    Text = "Home Page"
                }
            };

            return items;
        }
    }
}
