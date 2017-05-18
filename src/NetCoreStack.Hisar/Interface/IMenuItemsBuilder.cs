using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace NetCoreStack.Hisar
{
    public interface IMenuItemsBuilder
    {
        RunningComponentHelper Component { get; }
        string ResolvePath(IUrlHelper urlHelper, string path);
        IEnumerable<IMenuItem> Build(IUrlHelper urlHelper);
    }
}
