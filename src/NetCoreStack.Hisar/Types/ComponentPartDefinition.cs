using System.Reflection;

namespace NetCoreStack.Hisar
{
    internal class ComponentPartDefinition
    {
        public string ComponentId { get; set; }
        public string Name { get; set; }
        public Assembly Assembly { get; set; }
    }
}
