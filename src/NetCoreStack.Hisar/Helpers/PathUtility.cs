using System.IO;
using System.Linq;

namespace NetCoreStack.Hisar
{
    internal static class PathUtility
    {
        public static void CopyToFiles(string source, string destination)
        {
            foreach (string newPath in Directory.GetFiles(source, "*.*", SearchOption.AllDirectories)
                .Where(file => file.ToLower().EndsWith("dll") || file.ToLower().EndsWith("nupkg")))
            {
                File.Copy(newPath, newPath.Replace(source, destination), true);
            }
        }
    }
}
