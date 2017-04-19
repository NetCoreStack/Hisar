using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace NetCoreStack.Hisar
{
    public interface IDefaultProxyFileLocator
    {
        IFileInfo Layout { get; set; }
        IFileInfo GetFileInfo(string fullname);
        IChangeToken CreateFileChangeToken(string fullname);
        void RaiseChange(string fullname);
    }
}
