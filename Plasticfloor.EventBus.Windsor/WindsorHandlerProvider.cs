using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel;
using Castle.Windsor;

namespace PlasticFloor.EventBus.Windsor
{
    public class WindsorHandlerProvider : IHandlerProvider
    {
        private readonly IKernel _kernel;
        public WindsorHandlerProvider(IKernel kernel)
        {
            _kernel = kernel;
        }

        public IEnumerable<IEventHandler<TEvent>> GetHandlers<TEvent>() where TEvent : IEvent
        {
            return _kernel.ResolveAll<IEventHandler<TEvent>>();
        }

        public void ReleaseHandler(object handler)
        {
            _kernel.ReleaseComponent(handler);
        }
    }
}
