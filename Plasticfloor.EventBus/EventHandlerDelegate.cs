namespace PlasticFloor.EventBus
{
    public delegate void EventHandlerDelegate<in TEvent>(TEvent e) where TEvent : IEvent;
}