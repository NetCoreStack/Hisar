using System;
using System.Reflection;
using System.Runtime.Loader;

namespace NetCoreStack.Hisar
{
    public interface IAssemblyProviderResolveCallback
    {
        bool TryLoad(AssemblyLoadContext loadContext, Assembly entryAssembly, string fullPath, Exception ex);
    }
}
