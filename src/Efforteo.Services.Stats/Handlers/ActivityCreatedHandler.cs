using System;
using System.Threading.Tasks;
using Efforteo.Common.Commands;
using Efforteo.Common.Events;
using Efforteo.Common.Exceptions;
using Efforteo.Services.Stats.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RawRabbit;

namespace Efforteo.Services.Stats.Handlers
{
    public class ActivityCreatedHandler : IEventHandler<ActivityCreated>
    {
        private readonly IBusClient _busClient;
        private readonly IStatService _service;
        private readonly ILogger _logger;

        public ActivityCreatedHandler(IBusClient busClient, IStatService service, ILogger<ActivityCreatedHandler> logger)
        {
            _busClient = busClient;
            _service = service;
            _logger = logger;
        }

        public async Task HandleAsync(ActivityCreated command)
        {
            _logger.LogInformation($"Creating stats for activity={JsonConvert.SerializeObject(command)}");

            try
            {
                await _service.AddAsync(command.UserId, command.Id, command.Category, command.Distance, command.Time, command.CreatedAt);
                _logger.LogInformation($"Created stats for activity=(id={command.Id})");
            }
            catch (EfforteoException exception)
            {
                _logger.LogError($"Failed to create stats for activity=(id={command.Id}, code={exception.Code}, message={exception.Message})");
            }
            catch (Exception exception)
            {
                _logger.LogError($"Failed to create stats for activity=(id={command.Id}, code=unknown, message={exception.Message})");
            }
        }
    }
}