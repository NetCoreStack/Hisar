using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace NetCoreStack.Hisar
{
    public interface IDefaultLayoutFileProvider
    {
        IFileInfo Layout { get; set; }
        IChangeToken CreateFileChangeToken(string fullname);
        void RaiseChange(string fullname);
    }
}
