using System;

namespace NetCoreStack.Hisar
{
    public interface IComponentEventSubscriber
    {
        bool TryGetEventHandler<TEvent>(out Action<TEvent> handler);
    }
}
