using System;
using System.Reflection;

namespace NetCoreStack.Hisar
{
    public enum ComponentType
    {
        External = 0,
        Hosting = 1
    }

    public class RunningComponentDefinition
    {
        public Type StartupType { get; }
        public ComponentType ComponentType { get; }
        public string ComponentId { get; }
        public RunningComponentDefinition(string componentId, Type startupType, ComponentType componentType)
        {
            ComponentId = componentId;
            StartupType = startupType;
            ComponentType = componentType;
        }
    }

    public abstract class RunningComponentHelper
    {
        public bool IsExternalComponent { get; }

        public Type StartupType { get; }

        public string ComponentId { get; }

        public RunningComponentHelper()
        {
            var definition = ResolveRunningComponent();
            IsExternalComponent = definition.ComponentType == ComponentType.External;
            StartupType = definition.StartupType;
            ComponentId = definition.ComponentId;
        }

        protected abstract RunningComponentDefinition ResolveRunningComponent();
    }

    public class RunningComponentHelperOfT<TStartup> : RunningComponentHelper
    {
        public RunningComponentHelperOfT()
        {
        }

        protected override RunningComponentDefinition ResolveRunningComponent()
        {
            var startupType = typeof(TStartup);
            var componentType = ComponentType.External;
            var assembly = startupType.GetTypeInfo().Assembly;
            var componentId = assembly.GetComponentId();

            if (componentId.Equals(EngineConstants.HostingComponentName, StringComparison.OrdinalIgnoreCase))
                componentType = ComponentType.Hosting;

            return new RunningComponentDefinition(componentId, startupType, componentType);
        }
    }
}