namespace NetCoreStack.Hisar
{
    public class PreWebPackObject : WebPackObject, IPreWebPackObject
    {
        public PreWebPackObject(WebDecoratorNames decorator,
            string path, 
            string fallbackPath,
            object attributes = null)
            :base(decorator, WebPackSection.Pre, path, fallbackPath, attributes)
        {
        }
    }
}