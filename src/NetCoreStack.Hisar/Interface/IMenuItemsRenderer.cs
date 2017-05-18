using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace NetCoreStack.Hisar
{
    public interface IMenuItemsRenderer
    {
        IDictionary<ComponentPair, IEnumerable<IMenuItem>> PopulateMenuItems(ActionContext context);

        IHtmlContent Render(ActionContext context, Action<IDictionary<ComponentPair, IEnumerable<IMenuItem>>> filter = null);
    }
}
