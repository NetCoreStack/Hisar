using System.Collections.Generic;


namespace Hisar.Component.CoreManagement.Models
{
    public class ComponentMethodDescriptor
    {
        public ComponentMethodDescriptor()
        {
            MethodParameters = new List<ComponentMethodParameterDescriptor>();
        }
        public string Name { get; set; }
        public string ReturnType { get; set; }
        public List<ComponentMethodParameterDescriptor> MethodParameters{ get; set; }
    }
}
