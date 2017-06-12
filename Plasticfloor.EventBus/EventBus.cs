using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlasticFloor.EventBus
{
    /// <summary>
    /// Default implementation of IEventBus
    /// </summary>
    public class EventBus : IEventBus
    {
        private readonly Dictionary<Type, List<Delegate>> _handlers = new Dictionary<Type, List<Delegate>>();
        private readonly List<IHandlerProvider> _handlerProviders = new List<IHandlerProvider>();

        /// <summary>
        /// Register an instance of IHandlerProvider which will provide handlers for raised events.
        /// Multiple handler providers may be registered.
        /// </summary>
        /// <param name="handlerProvider"></param>
        public void RegisterProvider(IHandlerProvider handlerProvider)
        {
            if(handlerProvider==null) throw new ArgumentNullException(nameof(handlerProvider));
            if (_handlerProviders.Contains(handlerProvider)) return;
            _handlerProviders.Add(handlerProvider);
        }

        /// <summary>
        /// Register an EventHandlerDelegate&lt;TEvent&gt; to call when events of type &lt;TEvent&gt;
        /// or its subtypes are raised. Multiple handlers for types may be registered.
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="handlers"></param>
        public void Register<TEvent>(params EventHandlerDelegate<TEvent>[] handlers) where TEvent : IEvent
        {
            if (!_handlers.ContainsKey(typeof(TEvent))) _handlers.Add(typeof(TEvent), new List<Delegate>());
            foreach (var handler in handlers)
                _handlers[typeof(TEvent)].Add(handler);
        }

        private void Handle<TEvent>(TEvent @event) where TEvent : IEvent
        {
            if(@event==null) throw new ArgumentException(nameof(@event));
            HandleWithRegisteredDelegates(@event);
            HandleWithHandlerProviders(@event);
        }

        private void HandleWithRegisteredDelegates<TEvent>(TEvent @event, bool safe = false) where TEvent : IEvent
        {
            if (_handlers.ContainsKey(typeof(TEvent)))
                _handlers[typeof(TEvent)].ForEach(h => ExecuteHandler(((EventHandlerDelegate<TEvent>)h), @event, safe));
        }

        private void HandleWithHandlerProviders<TEvent>(TEvent e, bool safe = false) where TEvent : IEvent
        {
            _handlerProviders.ForEach(handlerProvider =>
            {
                var providedHandlers = handlerProvider.GetHandlers<TEvent>();
                foreach (var providedHandler in providedHandlers)
                {
                    ExecuteHandler(providedHandler.HandleEvent, e, safe);
                    handlerProvider.ReleaseHandler(providedHandler);
                }
            });
        }

        private void ExecuteHandler<TEvent>(EventHandlerDelegate<TEvent> handler, TEvent @event, bool safe) where TEvent : IEvent
        {
            if (safe)
                try
                {
                    handler.Invoke(@event);
                }
                catch (Exception ex)
                {
                    try
                    {
                        Handle(new ExceptionEvent(ex));
                    }
                    catch
                    {
                        // ignored - this is if an exception handler throws an exception.
                    }
                }
            else
                handler.Invoke(@event);
        }

        private void Handle<TEvent>(TEvent @event, bool safe) where TEvent : IEvent
        {
            HandleWithRegisteredDelegates(@event, safe);
            HandleWithHandlerProviders(@event, safe);
        }

        /// <summary>
        /// Raise an event to be handled by registered event handlers.
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="event"></param>
        public void Raise<TEvent>(TEvent @event) where TEvent : IEvent
        {
            if(@event==null) throw new ArgumentNullException(nameof(@event));
            Handle(@event, false);
        }

        /// <summary>
        /// Raise an event to be handled by registered event handlers. Any exceptions thrown 
        /// by event handlers are converted to an ExceptionEvent which is re-raised to be handled
        /// by any registered handlers for ExceptionEvent. The caller is "safe" from any exceptions
        /// thrown by handling of the event.
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="event"></param>
        public void RaiseSafely<TEvent>(TEvent @event) where TEvent : IEvent
        {
            if (@event == null) throw new ArgumentNullException(nameof(@event));
            Handle(@event, true);
        }

        public bool HasHandler<TEvent>() where TEvent : IEvent
        {
            return _handlers.ContainsKey(typeof(TEvent))
                   || _handlerProviders.Any(h => h.GetHandlers<TEvent>().Any());
        }
    }
}
