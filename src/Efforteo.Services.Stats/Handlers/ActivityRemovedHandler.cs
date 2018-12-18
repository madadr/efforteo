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
    public class ActivityRemovedHandler : IEventHandler<ActivityRemoved>
    {
        private readonly IBusClient _busClient;
        private readonly IStatService _service;
        private readonly ILogger _logger;

        public ActivityRemovedHandler(IBusClient busClient, IStatService service, ILogger<ActivityRemovedHandler> logger)
        {
            _busClient = busClient;
            _service = service;
            _logger = logger;
        }

        public async Task HandleAsync(ActivityRemoved command)
        {
            _logger.LogInformation($"Removing stats for activity={JsonConvert.SerializeObject(command)}");

            try
            {
                await _service.RemoveAsync(command.Id);
                _logger.LogInformation($"Removed stats for activity=(id={command.Id})");
            }
            catch (EfforteoException exception)
            {
                _logger.LogError($"Failed to remove stats for activity=(id={command.Id}, code={exception.Code}, message={exception.Message})");
            }
            catch (Exception exception)
            {
                _logger.LogError($"Failed to remove stats for activity=(id={command.Id}, code=unknown, message={exception.Message})");
            }
        }
    }
}