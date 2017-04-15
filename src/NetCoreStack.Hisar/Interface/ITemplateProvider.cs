namespace NetCoreStack.Hisar
{
    public interface ITemplateProvider
    {
        string Name { get; }
        ILayoutFactory LayoutFactory { get; }
    }
}