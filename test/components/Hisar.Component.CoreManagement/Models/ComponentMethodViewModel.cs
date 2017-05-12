using System.Collections.Generic;


namespace Hisar.Component.CoreManagement.Models
{
    public class ComponentMethodViewModel
    {
        public ComponentMethodViewModel()
        {
            MethodParameters = new List<ComponentMethodParameterViewModel>();
        }
        public string Name { get; set; }
        public string ReturnType { get; set; }
        public List<ComponentMethodParameterViewModel> MethodParameters{ get; set; }
    }
}
