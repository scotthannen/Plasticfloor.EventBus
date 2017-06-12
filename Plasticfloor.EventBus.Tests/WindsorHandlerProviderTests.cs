using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlasticFloor.EventBus.Windsor;

namespace PlasticFloor.EventBus.Tests
{
    [TestClass]
    public class WindsorHandlerProviderTests
    {
        private EventBus _eventBus;
        private IWindsorContainer _container;
        private EventAHandler _eventAHandler;

        [TestInitialize]
        public void Setup()
        {
            _container = new WindsorContainer();
            var handlerProvider = new WindsorHandlerProvider(_container.Kernel);
            _eventBus = new EventBus();
            _eventBus.RegisterProvider(handlerProvider);
            _eventAHandler = new EventAHandler();
            _container.Register(
                Component.For<IEventHandler<EventA>>().Instance(_eventAHandler)
            );
        }

        [TestMethod]
        public void CallsHandlersResolvedByHandlerProvider()
        {
            var events = new List<EventA> { new EventA(), new EventA(), new EventA() };
            events.ForEach(e => _eventBus.Raise(e));
            Assert.IsTrue(_eventAHandler.Values.SequenceEqual(events.Select(e => e.Value)));
        }

        [TestMethod]
        public void CallsHandlersResolvedByHandlerProviderAndRegisteredHandlers()
        {
            var events = new List<EventA> { new EventA(), new EventA(), new EventA() };
            var secondEventHandler = new EventAHandler();
            _eventBus.Register<EventA>(secondEventHandler.HandleEvent);
            events.ForEach(e => _eventBus.Raise(e));
            Assert.IsTrue(_eventAHandler.Values.SequenceEqual(events.Select(e => e.Value)));
            Assert.IsTrue(secondEventHandler.Values.SequenceEqual(events.Select(e => e.Value)));
        }

        [TestCleanup]
        public void Cleanup()
        {
           //_container.Dispose();
        }
    }
}