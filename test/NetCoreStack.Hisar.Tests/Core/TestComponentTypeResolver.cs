namespace NetCoreStack.Hisar.Tests.Core
{
    public class TestComponentTypeResolver : IComponentTypeResolver
    {
        public ComponentType Resolve(string componentId)
        {
            return ComponentType.Hosting;
        }
    }
}