using Microsoft.AspNetCore.Mvc;
using NetCoreStack.Hisar;
using System.Collections.Generic;

namespace Admin.Hosting
{
    public class HostingMenuBuilder : DefaultMenuItemsBuilder<Startup>
    {
        public HostingMenuBuilder(IComponentTypeResolver componentTypeResolver)
            : base(componentTypeResolver)
        {
        }

        public override IEnumerable<IMenuItem> Build(IUrlHelper urlHelper)
        {
            var items = new List<IMenuItem>
            {
                new DefaultMenuItem
                {
                    Icon = FontAwesomeIcon.Home,
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
