using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using NetCoreStack.Hisar;
using System.Collections.Generic;

namespace Hisar.Component.Guideline
{
    public class CustomMenuBuilder : DefaultMenuItemsBuilder<Startup>
    {
        public CustomMenuBuilder(IUrlHelperFactory urlHelperFactory, IComponentTypeResolver componentTypeResolver)
            : base(urlHelperFactory, componentTypeResolver)
        {
        }

        public override IEnumerable<IMenuItem> Build(ActionContext context)
        {
            var urlHelper = UrlHelperFactory.GetUrlHelper(context);

            var items = new List<IMenuItem>
            {
                new DefaultMenuItem
                {
                    Icon = FontAwesomeIcon.Home,
                    Order = 1,
                    Path = urlHelper.ComponentContent("~/"),
                    ShowInMenu = true,
                    Text = "Home Page"
                },
                new DefaultMenuItem
                {
                    Icon = FontAwesomeIcon.Home,
                    Order = 1,
                    Path = urlHelper.ComponentContent("~/Home/Albums"),
                    ShowInMenu = true,
                    Text = "Albums"
                }
            };

            return items;
        }
    }
}
