using Microsoft.Extensions.FileProviders;
using System.Reflection;

namespace NetCoreStack.Hisar
{
    public class DefaultAssemblyFileProviderFactory : IAssemblyFileProviderFactory
    {
        public IFileProvider CreateFileProvider(Assembly assembly)
        {
            var assemblyName = assembly.GetName().Name;
            return new DirectoryFriendlyEmbeddedFileProvider(assembly, assemblyName + ".wwwroot");
        }
    }
}
