using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hisar.Component.CoreManagement.Models;

namespace Hisar.Component.CoreManagement.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            
            return View();
        }

        [HttpGet]
        public IActionResult GetAssemblies()
        {
            var assemblyList = new List<AssemblyViewModel>()
            {
                new AssemblyViewModel()
                {
                    PackageId= "CarouselApp",
                    PackageVersion= "1.2.3.4",
                    Authors= "Gencebay Demir",
                    Company= "Bilge Adam Bilişim Hiz. Ltd. Şti.",
                    Product= "Carousel",
                    Description= "NetcoreStack Hisar Carousel",
                    Copyright= "Opengl Licence",
                    LicenceUrl= "http://opengl.com",
                    ProjectUrl= "https://github.com/NetCoreStack/Hisar",
                    IconUrl= "https://cdn0.iconfinder.com/data/icons/summer-2/128/Summer_512px-04.png",
                    RepositoryUrl= "https://github.com/NetCoreStack/Hisar",
                    Tags= "Hisar,Carousel,Slider,Plugin",
                    ReleaseNotes= "<section><h2>Your placeholder text :</h2><p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed ad rem redeamus; Sumenda potius quam expetenda. <mark>Maximus dolor, inquit, brevis est.</mark> Duo Reges: constructio interrete.</p><ol><li>Atque haec ita iustitiae propria sunt, ut sint virtutum reliquarum communia.</li><li>Hoc simile tandem est?</li><li>Ad eas enim res ab Epicuro praecepta dantur.</li><li>Ut in voluptate sit, qui epuletur, in dolore, qui torqueatur.</li></ol><pre> 	 Quae quod Aristoni et Pyrrhoni omnino visa sunt pro nihilo, 	 ut inter optime valere et gravissime aegrotare nihil prorsus 	 dicerent interesse, recte iam pridem contra eos desitum est 	 disputari. 	 Contineo me ab exemplis. 	 </pre></section>",
                    NeutrelLanguage= "Tuskish,English",
                    Version= "2.0.0.0",
                    FileVersion= "3.0.0.0",
                    Components = new List<ComponentViewModel>()
                    {
                        new ComponentViewModel()
                        {
                            Inherited = "Controller",
                            Name = "CarouselController",
                            ComponentMethods = new List<ComponentMethodViewModel>()
                            {
                                new ComponentMethodViewModel()
                                {
                                    Name = "GetCarousel",
                                    ReturnType = "IActionResult",
                                    MethodParameters = new List<ComponentMethodParameterViewModel>()
                                    {
                                        new ComponentMethodParameterViewModel
                                        {
                                            ParameterName = "categoryId",
                                            ParameterType = "long",

                                        }
                                    }
                                }
                            }
                        },
                        new ComponentViewModel()
                        {
                            Inherited = "Controller",
                            Name = "SampleController",
                            ComponentMethods = new List<ComponentMethodViewModel>()
                            {
                                new ComponentMethodViewModel()
                                {
                                    Name = "GetSample",
                                    ReturnType = "IActionResult",
                                    MethodParameters = new List<ComponentMethodParameterViewModel>()
                                    {
                                        new ComponentMethodParameterViewModel
                                        {
                                            ParameterName = "name",
                                            ParameterType = "string",
                                        },
                                        new ComponentMethodParameterViewModel
                                        {
                                            ParameterName = "id",
                                            ParameterType = "long",
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                ,
                new AssemblyViewModel()
                {
                    PackageId= "ComponentManagementApp",
                    PackageVersion= "1.2.3.4",
                    Authors= "Taha İPEK",
                    Company= "Bilge Adam Bilişim Hiz. Ltd. Şti.",
                    Product= "Component Management",
                    Description= "NetcoreStack Hisar Component Management",
                    Copyright= "Opengl Licence",
                    LicenceUrl= "http://opengl.com",
                    ProjectUrl= "https://github.com/NetCoreStack/Hisar",
                    IconUrl= "https://cdn0.iconfinder.com/data/icons/summer-2/128/Summer_512px-04.png",
                    RepositoryUrl= "https://github.com/NetCoreStack/Hisar",
                    Tags= "Hisar,Component,Management,Plugin",
                    ReleaseNotes= "<section><h2>Your placeholder text :</h2><p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed ad rem redeamus; Sumenda potius quam expetenda. <mark>Maximus dolor, inquit, brevis est.</mark> Duo Reges: constructio interrete.</p><ol><li>Atque haec ita iustitiae propria sunt, ut sint virtutum reliquarum communia.</li><li>Hoc simile tandem est?</li><li>Ad eas enim res ab Epicuro praecepta dantur.</li><li>Ut in voluptate sit, qui epuletur, in dolore, qui torqueatur.</li></ol><pre> 	 Quae quod Aristoni et Pyrrhoni omnino visa sunt pro nihilo, 	 ut inter optime valere et gravissime aegrotare nihil prorsus 	 dicerent interesse, recte iam pridem contra eos desitum est 	 disputari. 	 Contineo me ab exemplis. 	 </pre></section>",
                    NeutrelLanguage= "Tuskish,English",
                    Version= "2.0.0.0",
                    FileVersion= "3.0.0.0",
                    Components = new List<ComponentViewModel>()
                    {
                        new ComponentViewModel()
                        {
                            Inherited = "Controller",
                            Name = "HomeController",
                            ComponentMethods = new List<ComponentMethodViewModel>()
                            {
                                new ComponentMethodViewModel()
                                {
                                    Name = "GetAsseblies",
                                    ReturnType = "IActionResult"
                                }
                            }
                        }
                    }
                }
                ,
                new AssemblyViewModel()
                {
                    PackageId= "GuidelineApp",
                    PackageVersion= "1.2.3.4",
                    Authors= "Gencebay Demir",
                    Company= "Bilge Adam Bilişim Hiz. Ltd. Şti.",
                    Product= "Carousel",
                    Description= "NetcoreStack Hisar Guideline",
                    Copyright= "Opengl Licence",
                    LicenceUrl= "http://opengl.com",
                    ProjectUrl= "https://github.com/NetCoreStack/Hisar",
                    IconUrl= "https://cdn0.iconfinder.com/data/icons/summer-2/128/Summer_512px-04.png",
                    RepositoryUrl= "https://github.com/NetCoreStack/Hisar",
                    Tags= "Hisar,Guideline,Plugin",
                    ReleaseNotes= "<section><h2>Your placeholder text :</h2><p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed ad rem redeamus; Sumenda potius quam expetenda. <mark>Maximus dolor, inquit, brevis est.</mark> Duo Reges: constructio interrete.</p><ol><li>Atque haec ita iustitiae propria sunt, ut sint virtutum reliquarum communia.</li><li>Hoc simile tandem est?</li><li>Ad eas enim res ab Epicuro praecepta dantur.</li><li>Ut in voluptate sit, qui epuletur, in dolore, qui torqueatur.</li></ol><pre> 	 Quae quod Aristoni et Pyrrhoni omnino visa sunt pro nihilo, 	 ut inter optime valere et gravissime aegrotare nihil prorsus 	 dicerent interesse, recte iam pridem contra eos desitum est 	 disputari. 	 Contineo me ab exemplis. 	 </pre></section>",
                    NeutrelLanguage= "Tuskish,English",
                    Version= "2.0.0.0",
                    FileVersion= "3.0.0.0",
                    Components = new List<ComponentViewModel>()
                    {
                        new ComponentViewModel()
                        {
                            Inherited = "Controller",
                            Name = "GuidelineController",
                            ComponentMethods = new List<ComponentMethodViewModel>()
                            {
                                new ComponentMethodViewModel()
                                {
                                    Name = "GetGuideline",
                                    ReturnType = "IActionResult"
                                }
                            }
                        }
                    }
                }
            };

            return Json(assemblyList);
        }
    }
}
