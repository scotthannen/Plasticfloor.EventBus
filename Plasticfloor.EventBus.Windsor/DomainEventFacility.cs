using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.Registration;

namespace PlasticFloor.EventBus.Windsor
{
    /// <summary>
    /// Facility for using EventBus with Windsor IoC container. 
    /// Allows resolving IEventBus from the container. When an event
    /// raised on the returned IEventBus implementation, any matching
    /// implementations of IEventHander&lt;TEvent&gt; are resolved from
    /// the container and invoked to handle the event.
    /// </summary>
    public class DomainEventFacility : AbstractFacility
    {
        private WindsorHandlerProvider _windsorHandlerProvider;
        protected override void Init()
        {
            var eventBus = new EventBus();
            Kernel.Register(
                Component.For<IEventBus>()
                    .Instance(eventBus)
                    .LifestyleSingleton()
            );
            _windsorHandlerProvider = new WindsorHandlerProvider(Kernel);
            eventBus.RegisterProvider(_windsorHandlerProvider);
        }
    }
}