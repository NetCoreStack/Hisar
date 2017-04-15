using Microsoft.Extensions.FileProviders;

namespace NetCoreStack.Hisar
{
    public class DefaultLayoutFileProvider : IDefaultLayoutFileProvider
    {
        public IFileInfo Layout { get; set; }

        public DefaultLayoutFileProvider()
        {
        }
    }
}