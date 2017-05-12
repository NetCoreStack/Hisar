using System;
using System.Reflection;

namespace NetCoreStack.Hisar
{
    public enum ComponentType
    {
        External = 0,
        Hosting = 1,
        Core = 2
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

        public bool IsCoreComponent { get; }

        public Type StartupType { get; }

        public string ComponentId { get; }

        public IComponentTypeResolver ComponentTypeResolver { get; }

        public RunningComponentHelper(IComponentTypeResolver resolver)
        {
            ComponentTypeResolver = resolver;
            var definition = ResolveRunningComponent();

            IsExternalComponent = definition.ComponentType == ComponentType.External ||
                definition.ComponentType == ComponentType.Core;

            IsCoreComponent = definition.ComponentType == ComponentType.Core;
            StartupType = definition.StartupType;
            ComponentId = definition.ComponentId;
        }

        protected abstract RunningComponentDefinition ResolveRunningComponent();
    }

    public class RunningComponentHelperOfT<TStartup> : RunningComponentHelper
    {
        public RunningComponentHelperOfT(IComponentTypeResolver resolver)
            :base(resolver)
        {
        }

        protected override RunningComponentDefinition ResolveRunningComponent()
        {
            var startupType = typeof(TStartup);
            var assembly = startupType.GetTypeInfo().Assembly;
            var componentId = assembly.GetComponentId();
            var componentType = ComponentTypeResolver.Resolve(componentId);

            return new RunningComponentDefinition(componentId, startupType, componentType);
        }
    }
}