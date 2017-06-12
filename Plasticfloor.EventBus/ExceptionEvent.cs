using System;

namespace PlasticFloor.EventBus
{
    /// <summary>
    /// Event representing an Exception. ExceptionEvent may be used to communicate exceptions
    /// for logging. When EventBus.RaiseSafely(event) results in an exception, an ExceptionEvent 
    /// containing that exception is raised so that any registered handlers for ExceptionEvent
    /// are executed.
    /// </summary>
    public class ExceptionEvent : IEvent
    {
        private readonly Exception _exception;

        public Exception Exception { get { return _exception; } }

        public ExceptionEvent(Exception exception, string message = null)
        {
            if(exception==null) throw new ArgumentNullException(nameof(exception));
            if (string.IsNullOrEmpty(message))
                _exception = exception;
            else
                _exception = new Exception(message, exception);
        }
    }
}