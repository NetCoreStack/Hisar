using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;

namespace NetCoreStack.Hisar
{
    internal class NameSpaceRoutingConvention : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            foreach (var controllerModel in application.Controllers)
            {
                var componentId = controllerModel.ControllerType.Assembly.GetComponentId();
                if (!componentId.Equals(EngineConstants.HostingComponentName, StringComparison.OrdinalIgnoreCase))
                {
                    var attribute = new HisarRouteAttribute(componentId);
                    var routeModel = new AttributeRouteModel(attribute)
                    {
                        Name = componentId
                    };

                    controllerModel.Selectors[0].AttributeRouteModel = routeModel;
                    if (controllerModel.Attributes is List<object> attributes)
                    {
                        attributes.Add(attribute);
                    }

                    if (!controllerModel.RouteValues.ContainsKey("area"))
                    {
                        controllerModel.RouteValues.Add("area", componentId);
                    }
                }                
            }
        }
    }
}