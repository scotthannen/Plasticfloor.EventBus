using System.Collections.Generic;

namespace PlasticFloor.EventBus
{
    public interface IHandlerProvider
    {
        IEnumerable<IEventHandler<TEvent>> GetHandlers<TEvent>() where TEvent : IEvent;
        void ReleaseHandler(object handler);
    }
}