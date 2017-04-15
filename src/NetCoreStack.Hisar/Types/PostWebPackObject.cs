using System.Collections.Generic;

namespace NetCoreStack.Hisar
{
    public class PostWebPackObject : WebPackObject, IPostWebPackObject
    {
        public PostWebPackObject(WebDecoratorNames decorator,
            string path,
            string fallbackPath,
            object attributes = null)
            : base(decorator, WebPackSection.Post, path, fallbackPath, attributes)
        {
        }
    }

    public static class PostWebPackObjectExtensions
    {
        public static void AddPostScripts(this IList<IPostWebPackObject> webpacks, params string[] scripts)
        {
            if (scripts != null)
            {
                foreach (var script in scripts)
                {
                    webpacks.Add(new PostWebPackObject(WebDecoratorNames.Script, script, ""));
                }
            }
        }
    }
}