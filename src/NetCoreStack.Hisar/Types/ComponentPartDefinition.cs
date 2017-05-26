using System;
using System.Reflection;

namespace NetCoreStack.Hisar
{
    internal class ComponentPartDefinition
    {
        public string ComponentId { get; set; }
        public string Name { get; set; }
        public Assembly Assembly { get; set; }
    }

    public sealed class ComponentPair : IEquatable<ComponentPair>
    {
        public string ComponentId { get; }
        public ComponentType ComponentType { get; }

        public ComponentPair(string componentId, ComponentType componentType)
        {
            ComponentId = componentId;
            ComponentType = componentType;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ComponentPair);
        }

        public override int GetHashCode()
        {
            return ComponentId.GetHashCode();
        }

        public bool Equals(ComponentPair other)
        {
            return other != null && other.ComponentId.Equals(ComponentId);
        }
    }
}
