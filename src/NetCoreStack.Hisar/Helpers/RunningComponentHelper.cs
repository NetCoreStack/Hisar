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
        public string Title { get; }
        public RunningComponentDefinition(string componentId, string title, Type startupType, ComponentType componentType)
        {
            ComponentId = componentId;
            Title = title;
            StartupType = startupType;
            ComponentType = componentType;
        }
    }

    public abstract class RunningComponentHelper
    {
        public bool IsExternalComponent { get; }

        public bool IsCoreComponent { get; }

        public Type StartupType { get; }

        public string Title { get; }
        public string ComponentId { get; }

        public ComponentType ComponentType { get; }

        public IComponentTypeResolver ComponentTypeResolver { get; }

        public RunningComponentHelper(IComponentTypeResolver resolver)
        {
            ComponentTypeResolver = resolver;
            var definition = ResolveRunningComponent();

            IsExternalComponent = definition.ComponentType == ComponentType.External ||
                definition.ComponentType == ComponentType.Core;

            IsCoreComponent = definition.ComponentType == ComponentType.Core;
            ComponentType = definition.ComponentType;
            StartupType = definition.StartupType;
            ComponentId = definition.ComponentId;
            Title = definition.Title;
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
            var title = assembly.TryGetAssemblyTitle();
            var componentType = ComponentTypeResolver.Resolve(componentId);

            return new RunningComponentDefinition(componentId, title, startupType, componentType);
        }
    }
}