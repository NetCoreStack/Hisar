using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Linq;

namespace NetCoreStack.Hisar
{
    internal class NameSpaceRoutingConvention : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                foreach (var action in controller.Actions)
                {
                    var selectorIndex = 0;
                    if (action.ActionName == "Index")
                    {
                        action.Selectors[0].AttributeRouteModel = new AttributeRouteModel()
                        {
                            Template = ""
                        };

                        action.Selectors.Add(new SelectorModel(action.Selectors[0])
                        {
                            AttributeRouteModel = new AttributeRouteModel()
                            {
                                Template = "[controller]"
                            }
                        });

                        selectorIndex++;
                    }

                    var attributeRouteModel = new AttributeRouteModel()
                    {
                        Template = "[controller]/[action]"
                    };

                    if (selectorIndex > 0)
                    {
                        action.Selectors.Add(new SelectorModel(action.Selectors[0])
                        {
                            AttributeRouteModel = attributeRouteModel
                        });
                    }
                    else
                        action.Selectors[selectorIndex].AttributeRouteModel = attributeRouteModel;
                }

                var hasAttributeRouteModels = controller.Selectors.Any(selector => selector.AttributeRouteModel != null);
                if (hasAttributeRouteModels)
                {
                    var hisarAttribute = controller.Selectors.Where(x => x.AttributeRouteModel.Attribute is HisarRouteAttribute).ToList();
                    if (hisarAttribute != null)
                    {
                        hisarAttribute[0].AttributeRouteModel = new AttributeRouteModel()
                        {
                            Template = ""
                        };
                    }
                }
            }
        }
    }
}