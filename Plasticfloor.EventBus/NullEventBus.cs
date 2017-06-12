namespace PlasticFloor.EventBus
{
    /// <summary>
    /// Null implementation of IEventBus. Ignores raised events.
    /// </summary>
    public class NullEventBus : IEventBus
    {
        public void Raise<TEvent>(TEvent @event) where TEvent : IEvent
        {}

        public void RaiseSafely<TEvent>(TEvent @event) where TEvent : IEvent
        {}
    }
}