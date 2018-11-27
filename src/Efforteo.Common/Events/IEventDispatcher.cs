using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace Efforteo.Common.Events
{
    public interface IEventDispatcher
    {
        Task DispatchAsync<T>(T @event) where T : IEvent;
    }
}
