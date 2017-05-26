using Microsoft.AspNetCore.Mvc;
using NetCoreStack.Hisar;
using System.Collections.Generic;

namespace Hisar.Component.FileManager
{
    public class CustomMenuBuilder : DefaultMenuItemsBuilder<Startup>
    {
        public override IEnumerable<IMenuItem> Build(IUrlHelper urlHelper)
        {
            return new List<IMenuItem>
            {
                new DefaultMenuItem
                {
                    Text = "Manage Files",
                    Path = ResolvePath(urlHelper, "~/"),
                    ShowInMenu = true
                }
            };
        }
    }
}
