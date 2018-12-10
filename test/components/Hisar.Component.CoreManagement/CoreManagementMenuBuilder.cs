using NetCoreStack.Hisar;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Hisar.Component.CoreManagement
{
    public class CoreManagementMenuBuilder : DefaultMenuItemsBuilder<Startup>
    {
        public override IEnumerable<IMenuItem> Build(IUrlHelper urlHelper)
        {
            var items = new List<IMenuItem>()
            {
                new DefaultMenuItem
                {
                    Icon = "cubes",
                    Order = 4,
                    Path = ResolvePath(urlHelper, "~/"),
                    ShowInMenu = true,
                    Text = "Bileşen Yönetimi"
                }
            };

            return items;
        }
    }
}
