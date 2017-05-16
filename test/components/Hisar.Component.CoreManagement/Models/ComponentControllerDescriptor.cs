using System.Collections.Generic;

namespace Hisar.Component.CoreManagement.Models
{
    public class ComponentControllerDescriptor
    {
        public ComponentControllerDescriptor()
        {
            ComponentMethods = new List<ComponentMethodDescriptor>();
        }
        public string Name { get; set; }
        public string Inherited { get; set; }
        public List<ComponentMethodDescriptor> ComponentMethods { get; set; }
    }
}
