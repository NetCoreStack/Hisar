using Microsoft.Extensions.FileProviders;
using System.Reflection;

namespace NetCoreStack.Hisar
{
    public interface IAssemblyFileProviderFactory
    {
        IFileProvider CreateFileProvider(Assembly assembly);
    }
}
