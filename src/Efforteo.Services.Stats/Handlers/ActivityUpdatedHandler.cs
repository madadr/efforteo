using System;
using System.Threading.Tasks;
using Efforteo.Common.Commands;
using Efforteo.Common.Events;
using Efforteo.Common.Exceptions;
using Efforteo.Services.Stats.Domain.DTO;
using Efforteo.Services.Stats.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RawRabbit;

namespace Efforteo.Services.Stats.Handlers
{
    public class ActivityUpdatedHandler : IEventHandler<ActivityUpdated>
    {
        private readonly IBusClient _busClient;
        private readonly IStatService _service;
        private readonly ILogger _logger;

        public ActivityUpdatedHandler(IBusClient busClient, IStatService service,
            ILogger<ActivityUpdatedHandler> logger)
        {
            _busClient = busClient;
            _service = service;
            _logger = logger;
        }

        public async Task HandleAsync(ActivityUpdated command)
        {
            _logger.LogInformation($"Updating stats for activity={JsonConvert.SerializeObject(command)}");

            try
            {
                await _service.UpdateAsync(new StatDto()
                {
                    Id = command.Id,
                    Category = command.Category,
                    Distance = command.Distance,
                    Time = command.Time,
                    CreatedAt = command.CreatedAt
                });
                _logger.LogInformation($"Updated stats for activity=(id={command.Id})");
            }
            catch (EfforteoException exception)
            {
                _logger.LogError(
                    $"Failed to update stats for activity=(id={command.Id}, code={exception.Code}, message={exception.Message})");
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    $"Failed to update stats for activity=(id={command.Id}, code=unknown, message={exception.Message})");
            }
        }
    }
}