using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hisar.Component.CoreManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using NetCoreStack.Hisar;

namespace Hisar.Component.CoreManagement.Helpers
{
    public class ComponentModelBuilderHelper
    {
        private readonly ApplicationPartManager _partManager;
        private readonly List<IApplicationModelProvider> _providers;

        public ComponentModelBuilderHelper(ApplicationPartManager partManager, IEnumerable<IApplicationModelProvider> providers)
        {
            _partManager = partManager;
            _providers = providers.ToList();
        }

        private IEnumerable<TypeInfo> GetControllerTypes()
        {
            var feature = new ControllerFeature();
            _partManager.PopulateFeature(feature);
            return feature.Controllers;
        }

        protected internal ApplicationModel BuildApplicationModel()
        {
            var controllerTypes = GetControllerTypes();
            var context = new ApplicationModelProviderContext(controllerTypes);

            for (var i = 0; i < _providers.Count; i++)
                _providers[i].OnProvidersExecuting(context);

            for (var i = _providers.Count - 1; i >= 0; i--)
                _providers[i].OnProvidersExecuted(context);

            return context.Result;
        }

        public List<AssemblyDescriptor> GetLoadedAssemblyInformation()
        {
            var assemblyViewModels = new List<AssemblyDescriptor>();

            var applicationModel = BuildApplicationModel();
            if (applicationModel != null && applicationModel.Controllers.Any())
                foreach (var controller in applicationModel.Controllers)
                {
                    var assembly = controller.ControllerType.Assembly;
                    var assemblyInfo = new AssemblyInformationHelper(assembly);
                    var viewComponents = GetViewComponents(assembly);
                    var assemblyDescriptor = new AssemblyDescriptor
                    {
                        ComponentId     = assemblyInfo.ComponentId,
                        PackageId       = assemblyInfo.PackageId,
                        PackageVersion  = assemblyInfo.PackageVersion,
                        Authors         = assemblyInfo.Authors,
                        Company         = assemblyInfo.Company,
                        Product         = assemblyInfo.Product,
                        Description     = assemblyInfo.Description,
                        Copyright       = assemblyInfo.Copyright,
                        LicenceUrl      = assemblyInfo.LicenceUrl,
                        ProjectUrl      = assemblyInfo.ProjectUrl,
                        IconUrl         = assemblyInfo.IconUrl,
                        RepositoryUrl   = assemblyInfo.RepositoryUrl,
                        Tags            = assemblyInfo.Tags,
                        ReleaseNotes    = assemblyInfo.ReleaseNotes,
                        NeutrelLanguage = assemblyInfo.NeutrelLanguage,
                        Version         = assemblyInfo.Version,
                        FileVersion     = assemblyInfo.FileVersion,
                        ViewComponents  = viewComponents
                    };
                    var avmComponent = new List<ComponentControllerDescriptor>(); // Controller

                    GetAssemblyController(ref avmComponent, controller);

                    assemblyDescriptor.Controllers = avmComponent;
                    assemblyViewModels.Add(assemblyDescriptor);
                }

            return assemblyViewModels;
        }

        private List<ComponentViewDescriptor> GetViewComponents(Assembly assembly)
        {
            var result = new List<ComponentViewDescriptor>();

            var componentId = assembly.GetComponentId();

            var assemblyTypes = assembly.GetTypes();
            if (!assemblyTypes.Any())
                return result;

            foreach (var type in assemblyTypes)
            {
                var componentType = type.GetTypeInfo();
                if (ViewComponentConventions.IsComponent(componentType))
                {
                    var attribute = componentType.GetCustomAttribute<ViewComponentAttribute>();
                    var componentName = !string.IsNullOrEmpty(attribute?.Name) ? attribute.Name : ViewComponentConventions.GetComponentName(componentType);
                    result.Add(new ComponentViewDescriptor()
                    {
                        Name = componentName,
                        ComponentId = componentId
                    });
                }
            }

            return result;

        }

        private void GetAssemblyController(ref List<ComponentControllerDescriptor> assemblyDescriptors, ControllerModel controllerModel)
        {
            var controllerMethods = new List<ComponentMethodDescriptor>();
            GetAssemblyControllerActions(ref controllerMethods, controllerModel);
            assemblyDescriptors.Add(new ComponentControllerDescriptor
            {
                Name = controllerModel.ControllerName,
                Inherited = string.Join(", ", controllerModel.ControllerType.ImplementedInterfaces.Select(k => k.Name)),
                ComponentMethods = controllerMethods
            });
        }

        private void GetAssemblyControllerActions(ref List<ComponentMethodDescriptor> componentMethodDescriptors, ControllerModel controllerModel)
        {
            foreach (var controllerModelAction in controllerModel.Actions)
            {
                var methodParameter = new List<ComponentMethodParameterDescriptor>();
                GetAssemblyControllerActionParameters(ref methodParameter, controllerModelAction);

                componentMethodDescriptors.Add(new ComponentMethodDescriptor
                {
                    Name = controllerModelAction.ActionName,
                    ReturnType = controllerModelAction.ActionMethod.ReturnType.Name,
                    MethodParameters = methodParameter
                });
            }
        }

        private void GetAssemblyControllerActionParameters(ref List<ComponentMethodParameterDescriptor> componentMethodParameterDescriptors, ActionModel actionModel)
        {
            foreach (var actionModelParameter in actionModel.Parameters)
                componentMethodParameterDescriptors.Add(new ComponentMethodParameterDescriptor
                {
                    ParameterType = actionModelParameter.ParameterInfo.ParameterType.ToString(),
                    ParameterName = actionModelParameter.ParameterName
                });
        }
    }
}