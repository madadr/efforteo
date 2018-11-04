using System;
using System.Threading.Tasks;
using Efforteo.Common.Events;

namespace Efforteo.Api.Handlers
{
    public class ActivityCreatedHandler : IEventHandler<ActivityCreated>
    {
        public async Task HandleAsync(ActivityCreated @event)
        {
            await Task.CompletedTask;
            Console.WriteLine($"Activity created {@event.Name}");
        }
    }
}
