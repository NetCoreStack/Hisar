using Microsoft.Extensions.FileProviders;

namespace NetCoreStack.Hisar
{
    public interface IDefaultLayoutFileProvider
    {
        IFileInfo Layout { get; set; }
    }
}
