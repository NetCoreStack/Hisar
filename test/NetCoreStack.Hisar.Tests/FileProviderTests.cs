using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;

namespace NetCoreStack.Hisar.Tests
{
    public class FileProviderTests : IClassFixture<HisarTestBase>
    {
        private readonly HisarTestBase _testBase;

        protected IServiceProvider ApplicationServices { get; }

        public FileProviderTests(HisarTestBase testBase)
        {
            _testBase = testBase;
            ApplicationServices = _testBase.ApplicationServices;
        }

        [Fact]
        public void GetComponentFileInfo()
        {
            var list = new List<string>
            {
                "/Views/Home/Components/Carousel/Carousel.Default.cshtml",
                "/Views/Shared/Components/Carousel/Carousel.Default.cshtml",
                "/Views/Shared/Components/Carousel/_ViewImports.cshtml",
                "/Views/Shared/Components/_ViewImports.cshtml",
                "/Views/Shared/_ViewImports.cshtml",
                "/_ViewImports.cshtml"
            };

            var loader = ApplicationServices.GetService<HisarAssemblyComponentsLoader>();
            var hisarEmbedFileProvider = new HisarEmbededFileProvider(loader.ComponentAssemblyLookup);

            foreach (var fullpath in list)
            {
                IFileInfo fileInfo = hisarEmbedFileProvider.GetFileInfo(fullpath);
                if (fileInfo == null)
                {

                }
            }
        }

        [Fact]
        public void GetControllerFileInfo()
        {
            var list = new List<string>
            {
                "/Areas/Guideline/Views/Home/Index.cshtml",
                "/Areas/Guideline/Views/Home/_ViewImports.cshtml",
                "/Areas/Guideline/Views/_ViewImports.cshtml",
                "/Areas/Guideline/_ViewImports.cshtml",
                "/Areas/_ViewImports.cshtml",
                "/_ViewImports.cshtml",
                "/Areas/Guideline/Views/Home/_ViewStart.cshtml",
                "/Areas/Guideline/Views/_ViewStart.cshtml",
                "/Areas/Guideline/_ViewStart.cshtml",
                "/Areas/_ViewStart.cshtml"
            };

            var loader = ApplicationServices.GetService<HisarAssemblyComponentsLoader>();
            var hisarEmbedFileProvider = new HisarEmbededFileProvider(loader.ComponentAssemblyLookup);

            foreach (var fullpath in list)
            {
                IFileInfo fileInfo = hisarEmbedFileProvider.GetFileInfo(fullpath);
                if (fileInfo == null)
                {

                }
            }
        }

        [Fact]
        public void GetComponentWithFullPath()
        {
            var list = new List<string>
            {
                "/Areas/Guideline/Views/Home/Components/MyTest/Guideline.Default.cshtml",
                "/Areas/Guideline/Views/Shared/Components/MyTest/Guideline.Default.cshtml",
                "/Views/Shared/Components/MyTest/Guideline.Default.cshtml",
                "/Views/Shared/Components/MyTest/_ViewImports.cshtml",
                "/Views/Shared/Components/_ViewImports.cshtml",
                "/Views/Shared/_ViewImports.cshtml",
                "/_ViewImports.cshtml"
            };

            var loader = ApplicationServices.GetService<HisarAssemblyComponentsLoader>();
            var hisarEmbedFileProvider = new HisarEmbededFileProvider(loader.ComponentAssemblyLookup);

            foreach (var fullpath in list)
            {
                IFileInfo fileInfo = hisarEmbedFileProvider.GetFileInfo(fullpath);
                if (fileInfo == null)
                {

                }
            }
        }
    }
}
