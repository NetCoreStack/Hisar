namespace NetCoreStack.Hisar
{
    public interface IComponentTypeResolver
    {
        ComponentType Resolve(string componentId);
    }
}
