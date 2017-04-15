using Microsoft.AspNetCore.Mvc.Routing;
using System;

namespace NetCoreStack.Hisar
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class HisarRouteAttribute : RouteValueAttribute, IRouteTemplateProvider
    {
        public string Name { get; }

        public int? Order { get; }

        public string Template { get; }

        public HisarRouteAttribute(string componentName)
            : base("area", componentName)
        {
            if (string.IsNullOrEmpty(componentName))
            {
                throw new ArgumentException("Component name must not be empty", nameof(componentName));
            }

            Name = componentName;
        }
    }
}