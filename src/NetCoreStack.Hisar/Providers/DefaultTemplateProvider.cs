namespace NetCoreStack.Hisar
{
    public class DefaultTemplateProvider : ITemplateProvider
    {
        public string Name { get; }

        public ILayoutFactory LayoutFactory { get; }

        public DefaultTemplateProvider(ILayoutFactory layoutFactory)
        {
            Name = "default";
            LayoutFactory = layoutFactory;
        }
    }
}