using System;
using System.Collections.Generic;

namespace PlasticFloor.EventBus.Tests
{
    public class EventA : IEquatable<EventA>, IEvent
    {
        public readonly Guid Value = Guid.NewGuid();

        public bool Equals(EventA other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EventA)obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }

    public class EventB : IEquatable<EventB>, IEvent
    {
        public readonly Guid Value = Guid.NewGuid();

        public bool Equals(EventB other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EventB)obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }

    public class EventInheritedFromB : EventB { }

    public class EventAHandler : IEventHandler<EventA>
    {
        public List<Guid> Values = new List<Guid>();
        public void HandleEvent(EventA e)
        {
            Values.Add(e.Value);
        }
    }

    public class SecondEventAHander : IEventHandler<EventA>
    {
        public List<Guid> Values = new List<Guid>();
        public void HandleEvent(EventA e)
        {
            Values.Add(e.Value);
        }
    }

    public class EventBHandler : IEventHandler<EventB>
    {
        public List<Guid> Values = new List<Guid>();
        public void HandleEvent(EventB e)
        {
            Values.Add(e.Value);
        }
    }

    public class EventInheritedFromBHandler : IEventHandler<EventInheritedFromB>
    {
        public List<Guid> Values = new List<Guid>();
        public void HandleEvent(EventInheritedFromB e)
        {
            Values.Add(e.Value);
        }
    }

    public class EvilEventHandler : IEventHandler<EventB>
    {
        public void HandleEvent(EventB e)
        {
            throw new Exception("Boo!");
        }
    }

    public class ExceptionHandler : IEventHandler<ExceptionEvent>
    {
        public List<string> ExceptionMessages = new List<string>();

        public void HandleEvent(ExceptionEvent e)
        {
            ExceptionMessages.Add(e.Exception.Message);
        }
    }

    public class EvilExeptionEventHandler : IEventHandler<ExceptionEvent>
    {
        public void HandleEvent(ExceptionEvent e)
        {
            throw new Exception("I'm evil, too!");
        }
    }
}

