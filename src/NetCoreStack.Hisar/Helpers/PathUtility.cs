using System.IO;

namespace NetCoreStack.Hisar
{
    internal static class PathUtility
    {
        public static void CopyToFiles(string source, string destination)
        {
            foreach (string dirPath in Directory.GetDirectories(source, "*", SearchOption.AllDirectories))
            {
                if (Path.GetFileName(dirPath) == "refs")
                    continue;

                Directory.CreateDirectory(dirPath.Replace(source, destination));
            }

            foreach (string newPath in Directory.GetFiles(source, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(source, destination), true);
            }
        }
    }
}
