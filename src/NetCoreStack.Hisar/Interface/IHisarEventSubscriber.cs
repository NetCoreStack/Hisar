using System;

namespace NetCoreStack.Hisar
{
    public interface IHisarEventSubscriber
    {
        bool TryGetEventHandler<TEvent>(out Action<TEvent> handler);
    }
}
