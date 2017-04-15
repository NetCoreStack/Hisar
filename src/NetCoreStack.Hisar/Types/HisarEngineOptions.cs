using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;

namespace NetCoreStack.Hisar
{
    public class HisarEngineOptions
    {
        public Type StartupType { get; set; }

        public List<Action<IRouteBuilder>> Routes { get; }

        public HisarEngineOptions()
        {
            Routes = new List<Action<IRouteBuilder>>();
        }
    }
}