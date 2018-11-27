using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Efforteo.Common.Events
{

    public class EventDispatcher : IEventDispatcher
    {
        private readonly IComponentContext _componentContext;

        public EventDispatcher(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        public async Task DispatchAsync<T>(T @event) where T : IEvent
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event), $"Event: '{typeof(T).Name}' cannot be null.");
            }

            var handler = _componentContext.Resolve<IEventHandler<T>>();
            await handler.HandleAsync(@event);
        }
    }
}
