using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Admin.Hosting
{
    public class CustomAssemblyResolveCallback : NetCoreStack.Hisar.IAssemblyProviderResolveCallback
    {
        public bool TryLoad(AssemblyLoadContext loadContext, Assembly entryAssembly, string fullPath, Exception ex)
        {
            return false;
        }
    }
}
