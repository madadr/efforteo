using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Efforteo.Common.Commands;
using Efforteo.Common.Events;
using RawRabbit;

namespace Efforteo.Services.Activities.Handlers
{
    public class CreateActivityHandler : ICommandHandler<CreateActivity>
    {
        private readonly IBusClient _busClient;

        public CreateActivityHandler(IBusClient busClient)
        {
            _busClient = busClient;
        }

        public async Task HandleAsync(CreateActivity command)
        {
            Console.WriteLine($"Creating activity... {command.Name}");
            await _busClient.PublishAsync(new ActivityCreated(command.UserId, command.Id, command.Category,
                command.Name, command.Description, DateTime.UtcNow));
            Console.WriteLine("Activity created, hooray!!!");
        }
    }
}