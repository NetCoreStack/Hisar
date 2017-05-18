using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace NetCoreStack.Hisar
{
    public interface IMenuItemsBuilder
    {
        RunningComponentHelper Component { get; }
        IEnumerable<IMenuItem> Build(ActionContext context);
    }
}
