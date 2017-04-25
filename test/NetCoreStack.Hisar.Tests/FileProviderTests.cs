using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.FileProviders;

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

        [Theory]
        [InlineData("/Views/Home/Components/Carousel/Carousel.Default.cshtml")]
        [InlineData("/Views/Shared/Components/Carousel/Carousel.Default.cshtml")]
        [InlineData("/Views/Shared/Components/Carousel/_ViewImports.cshtml")]
        [InlineData("/Views/Shared/Components/_ViewImports.cshtml")]
        [InlineData("/Views/Shared/_ViewImports.cshtml")]
        [InlineData("/_ViewImports.cshtml")]
        public void GetComponentFileInfo(string subpath)
        {
            var loader = ApplicationServices.GetService<HisarAssemblyComponentsLoader>();
            var hisarEmbedFileProvider = new HisarEmbededFileProvider(loader.ComponentAssemblyLookup);
            IFileInfo fileInfo = hisarEmbedFileProvider.GetFileInfo(subpath);
            if (fileInfo == null)
            {

            }
        }

        [Theory]
        [InlineData("/Areas/Guideline/Views/Home/Index.cshtml")]
        [InlineData("/Areas/Guideline/Views/Home/_ViewImports.cshtml")]
        [InlineData("/Areas/Guideline/Views/_ViewImports.cshtml")]
        [InlineData("/Areas/Guideline/_ViewImports.cshtml")]
        [InlineData("/Areas/_ViewImports.cshtml")]
        [InlineData("/_ViewImports.cshtml")]
        [InlineData("/Areas/Guideline/Views/Home/_ViewStart.cshtml")]
        [InlineData("/Areas/Guideline/Views/_ViewStart.cshtml")]
        [InlineData("/Areas/Guideline/_ViewStart.cshtml")]
        [InlineData("/Areas/_ViewStart.cshtml")]
        public void GetControllerFileInfo(string subpath)
        {
            var loader = ApplicationServices.GetService<HisarAssemblyComponentsLoader>();
            var hisarEmbedFileProvider = new HisarEmbededFileProvider(loader.ComponentAssemblyLookup);
            IFileInfo fileInfo = hisarEmbedFileProvider.GetFileInfo(subpath);
            if (fileInfo == null)
            {

            }
        }

        [Fact]
        [InlineData("/Areas/Guideline/Views/Home/Components/MyTest/Guideline.Default.cshtml")]
        [InlineData("/Areas/Guideline/Views/Shared/Components/MyTest/Guideline.Default.cshtml")]
        [InlineData("/Views/Shared/Components/MyTest/Guideline.Default.cshtml")]
        [InlineData("/Views/Shared/Components/MyTest/_ViewImports.cshtml")]
        [InlineData("/Views/Shared/Components/_ViewImports.cshtml")]
        [InlineData("/Views/Shared/_ViewImports.cshtml")]
        [InlineData("/_ViewImports.cshtml")]
        public void GetComponentWithFullPath(string subpath)
        {
            var loader = ApplicationServices.GetService<HisarAssemblyComponentsLoader>();
            var hisarEmbedFileProvider = new HisarEmbededFileProvider(loader.ComponentAssemblyLookup);
            IFileInfo fileInfo = hisarEmbedFileProvider.GetFileInfo(subpath);
            if (fileInfo == null)
            {

            }
        }
    }
}
