using System.Collections.Generic;

namespace Hisar.Component.CoreManagement.Models
{
    public class ComponentViewModel
    {
        public ComponentViewModel()
        {
            ComponentMethods = new List<ComponentMethodViewModel>();
        }
        public string Name { get; set; }
        public string Inherited { get; set; }
        public List<ComponentMethodViewModel> ComponentMethods { get; set; }
    }
}
