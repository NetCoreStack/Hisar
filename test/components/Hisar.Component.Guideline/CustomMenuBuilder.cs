﻿using Microsoft.AspNetCore.Mvc;
using NetCoreStack.Hisar;
using System.Collections.Generic;

namespace Hisar.Component.Guideline
{
    public class CustomMenuBuilder : DefaultMenuItemsBuilder<Startup>
    {
        public override IEnumerable<IMenuItem> Build(IUrlHelper urlHelper)
        {
            var items = new List<IMenuItem>
            {
                new DefaultMenuItem
                {
                    Icon = "home",
                    Order = 1,
                    Path = ResolvePath(urlHelper, "~/"),
                    ShowInMenu = true,
                    Text = "Home Page"
                },
                new DefaultMenuItem
                {
                    Icon = "inbox",
                    Order = 1,
                    Path = ResolvePath(urlHelper, "~/Home/Albums"),
                    ShowInMenu = true,
                    Text = "Albums"
                }
            };

            return items;
        }
    }
}
