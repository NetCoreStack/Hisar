using System.Linq;
using System.Reflection;

namespace NetCoreStack.Hisar
{
    public static class AssemblyExtensions
    {
        public static string GetComponentId(this Assembly componentAssembly)
        {
            return componentAssembly.GetName().Name.Split('.').Last();
        }
    }
}