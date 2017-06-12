using System;

namespace PlasticFloor.EventBus
{
    /// <summary>
    /// Static class for providing a static IEventBus instance.
    /// </summary>
    public class DomainEvents
    {
        private static IEventBus _eventBus = new NullEventBus();

        public static IEventBus EventBus => _eventBus;

        /// <summary>
        /// Sets an instance of IEventBus to be returned by the EventBus property.
        /// This can only bet set to replace the default instance (NullEventBus.)
        /// </summary>
        /// <param name="eventBus"></param>
        public void SetEventBus(IEventBus eventBus)
        {
            if(!(_eventBus is NullEventBus)) throw new InvalidOperationException("The IEventBus instance has already been set.");
            if(eventBus==null) throw new ArgumentNullException(nameof(eventBus));
            _eventBus = eventBus;
        }
    }
}