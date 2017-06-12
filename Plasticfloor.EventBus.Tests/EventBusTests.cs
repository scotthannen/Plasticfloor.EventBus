using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace PlasticFloor.EventBus.Tests
{
    [TestClass]
    public class EventBusTests
    {
        [TestMethod]
        public void CallsHandlersRegisteredAsDelegates()
        {
            var eventBus = new EventBus();
            var handler = new EventAHandler();
            var handler2 = new SecondEventAHander();
            eventBus.Register<EventA>(handler.HandleEvent);
            eventBus.Register<EventA>(handler2.HandleEvent);
            var events = new List<EventA> { new EventA(), new EventA(), new EventA() };
            events.ForEach(e => eventBus.Raise(e));
            Assert.IsTrue(handler.Values.SequenceEqual(events.Select(e => e.Value)));
            Assert.IsTrue(handler2.Values.SequenceEqual(events.Select(e => e.Value)));
        }

        [TestMethod]
        public void CallsHandlersForBaseClassesOfEvents()
        {
            var eventBus = new EventBus();
            var handler = new EventBHandler();
            eventBus.Register<EventB>(handler.HandleEvent);
            var events = new List<EventB> { new EventInheritedFromB(), new EventB() };
            events.ForEach(e => eventBus.Raise(e));
            Assert.IsTrue(handler.Values.SequenceEqual(events.Select(e => e.Value)));
         }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void RaiseEventThrowsExceptionFromEventHandler()
        {
            var eventBus = new EventBus();
            var handler = new EvilEventHandler();
            eventBus.Register<EventB>(handler.HandleEvent);
            eventBus.Raise(new EventB());
        }

        [TestMethod]
        public void RaiseSafelyDoesntThrowExceptionFromEventHandler()
        {
            var eventBus = new EventBus();
            var handler = new EvilEventHandler();
            eventBus.Register<EventB>(handler.HandleEvent);
            eventBus.RaiseSafely(new EventB());
        }

        [TestMethod]
        public void RegisteredExceptionHandlerCatchesExceptionWhenEventRaisedSafely()
        {
            var eventBus = new EventBus();
            var handler = new EvilEventHandler();
            var exceptionHandler = new ExceptionHandler();
            eventBus.Register<EventB>(handler.HandleEvent);
            eventBus.Register<ExceptionEvent>(exceptionHandler.HandleEvent);
            eventBus.RaiseSafely(new EventB());
            Assert.IsNotNull(exceptionHandler.ExceptionMessages.First(message => message == "Boo!"));
        }

        [TestMethod]
        public void RaiseSafelyProtectsAgainstExceptionsThrownByExceptionHandler()
        {
            var eventBus = new EventBus();
            var handler = new EvilEventHandler();
            var exceptionHandler = new EvilExeptionEventHandler();
            eventBus.Register<EventB>(handler.HandleEvent);
            eventBus.Register<ExceptionEvent>(exceptionHandler.HandleEvent);
            eventBus.RaiseSafely(new EventB());
            // No exceptions thrown.
        }

        [TestMethod]
        public void HandlerProviderHandlersGetReleased()
        {
            var handlerProvider = new Mock<IHandlerProvider>();
            var handler = new EventAHandler();
            handlerProvider.Setup(hp => hp.GetHandlers<EventA>())
                .Returns(new[] {handler})
                .Verifiable();
            var eventBus = new EventBus();
            eventBus.RegisterProvider(handlerProvider.Object);
            eventBus.Raise(new EventA());
            handlerProvider.Verify(hp=>hp.ReleaseHandler(handler));
        }
    }
}