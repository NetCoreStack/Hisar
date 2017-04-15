using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;

namespace NetCoreStack.Hisar
{
    public static class RouteExtensions
    {
        public static bool IsDefaultRoute(this Route route)
        {
            if (route == null)
            {
                throw new ArgumentNullException(nameof(route));
            }

            if (route.Defaults.Count == 2)
            {
                var query = route.Defaults.Where(x => x.Key.Equals("controller", StringComparison.OrdinalIgnoreCase) || 
                    x.Key.Equals("action", StringComparison.OrdinalIgnoreCase)).Count();

                return query == 2 && route.Name.Equals("default", StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
    }
}
