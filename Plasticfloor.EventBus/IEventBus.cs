namespace PlasticFloor.EventBus
{
    public interface IEventBus
    {
        void Raise<TEvent>(TEvent @event) where TEvent : IEvent;
        void RaiseSafely<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}