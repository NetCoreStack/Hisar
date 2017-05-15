using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Hisar.Component.CoreManagement.Models;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

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
            {
                _providers[i].OnProvidersExecuting(context);
            }

            for (var i = _providers.Count - 1; i >= 0; i--)
            {
                _providers[i].OnProvidersExecuted(context);
            }

            return context.Result;
        }






        public List<AssemblyViewModel> GetLoadedAssemblyInformation()
        {
            var assemblyViewModels = new List<AssemblyViewModel>();

            var applicationModel = BuildApplicationModel();
            if (applicationModel != null && applicationModel.Controllers.Any())
            {
                
                foreach (var controller in applicationModel.Controllers)
                {
                    //controller.ControllerType.Assembly;
                    var assemblyInfo = new AssemblyInformationHelper(controller.ControllerType.Assembly);

                    var avm = new AssemblyViewModel()
                    {
                        PackageId = assemblyInfo.PackageId,
                        PackageVersion = assemblyInfo.PackageVersion,
                        Authors = assemblyInfo.Authors,
                        Company = assemblyInfo.Company,
                        Product = assemblyInfo.Product,
                        Description = assemblyInfo.Description,
                        Copyright = assemblyInfo.Copyright,
                        LicenceUrl = assemblyInfo.LicenceUrl,
                        ProjectUrl = assemblyInfo.ProjectUrl,
                        IconUrl = assemblyInfo.IconUrl,
                        RepositoryUrl = assemblyInfo.RepositoryUrl,
                        Tags = assemblyInfo.Tags,
                        ReleaseNotes = assemblyInfo.ReleaseNotes,
                        NeutrelLanguage = assemblyInfo.NeutrelLanguage,
                        Version = assemblyInfo.Version,
                        FileVersion = assemblyInfo.FileVersion
                    };
                    var avmComponent = new List<ComponentViewModel>(); // Controller

                    GetAssemblyController(ref avmComponent, controller);

                    avm.Components = avmComponent;
                    assemblyViewModels.Add(avm);
                }

            }

            return assemblyViewModels;
        }



        private void GetAssemblyController(ref List<ComponentViewModel> componentViewModel, ControllerModel controllerModel)
        {
            var controllerMethods = new List<ComponentMethodViewModel>();
            GetAssemblyControllerActions(ref controllerMethods, controllerModel);
            componentViewModel.Add(new ComponentViewModel()
            {
                Name = controllerModel.ControllerName,
                Inherited = String.Join(", ", controllerModel.ControllerType.ImplementedInterfaces.Select(k => k.Name)),
                ComponentMethods = controllerMethods
            });
        }

        private void GetAssemblyControllerActions(ref List<ComponentMethodViewModel> componentMethodViewModel, ControllerModel controllerModel)
        {
            foreach (var controllerModelAction in controllerModel.Actions)
            {
                var methodParameter = new List<ComponentMethodParameterViewModel>();
                GetAssemblyControllerActionParameters(ref methodParameter, controllerModelAction);

                componentMethodViewModel.Add(new ComponentMethodViewModel
                {
                    Name = controllerModelAction.ActionName,
                    ReturnType = controllerModelAction.ActionMethod.ReturnType.Name,
                    MethodParameters = methodParameter
                });
            }
        }

        private void GetAssemblyControllerActionParameters(ref List<ComponentMethodParameterViewModel> componentMethodParameterViewModel, ActionModel actionModel)
        {
            foreach (var actionModelParameter in actionModel.Parameters)
            {
                componentMethodParameterViewModel.Add(new ComponentMethodParameterViewModel
                {
                    ParameterType = actionModelParameter.ParameterInfo.ParameterType.ToString(),
                    ParameterName = actionModelParameter.ParameterName
                });
            }
        }

    }
}
