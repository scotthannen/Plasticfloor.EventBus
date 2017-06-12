namespace PlasticFloor.EventBus
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        void HandleEvent(TEvent e);
    }
}