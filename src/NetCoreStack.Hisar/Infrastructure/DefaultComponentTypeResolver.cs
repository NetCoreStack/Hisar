using System;

namespace NetCoreStack.Hisar
{
    public class DefaultComponentTypeResolver : IComponentTypeResolver
    {
        public ComponentType Resolve(string componentId)
        {
            var componentType = ComponentType.External;

            if (componentId.Equals(EngineConstants.HostingComponentName, StringComparison.OrdinalIgnoreCase))
            {
                componentType = ComponentType.Hosting;
            }
            else if (componentId.StartsWith("Core"))
            {
                componentType = ComponentType.Core;
            }

            return componentType;
        }
    }
}
