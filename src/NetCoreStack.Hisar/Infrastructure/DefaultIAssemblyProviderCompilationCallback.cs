using System;
using System.Reflection;
using System.Runtime.Loader;

namespace NetCoreStack.Hisar
{
    public class DefaultIAssemblyProviderCompilationCallback : IAssemblyProviderResolveCallback
    {
        public bool TryLoad(AssemblyLoadContext loadContext, Assembly entryAssembly, string fullPath, Exception ex)
        {
            return false;
        }
    }
}
