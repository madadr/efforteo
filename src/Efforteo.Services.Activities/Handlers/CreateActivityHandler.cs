using System;
using System.Threading.Tasks;
using Efforteo.Common.Commands;
using Efforteo.Common.Events;
using Efforteo.Common.Exceptions;
using Efforteo.Services.Activities.Services;
using Microsoft.Extensions.Logging;
using RawRabbit;

namespace Efforteo.Services.Activities.Handlers
{
    public class CreateActivityHandler : ICommandHandler<CreateActivity>
    {
        private readonly IBusClient _busClient;
        private readonly IActivityService _activityService;
        private readonly ILogger _logger;

        public CreateActivityHandler(IBusClient busClient, IActivityService activityService, ILogger<CreateActivityHandler> logger)
        {
            _busClient = busClient;
            _activityService = activityService;
            _logger = logger;
        }

        public async Task HandleAsync(CreateActivity command)
        {
            _logger.LogInformation($"Creating activity... {command.Title}");

            try
            {
                await _activityService.AddAsync(command.UserId, command.Id, command.Category,
                    command.Title, command.Description, command.Time, command.Distance);
                await _busClient.PublishAsync(new ActivityCreated(command.UserId, command.Id, command.Category,
                    command.Title, command.Description, command.Time, command.Distance));
                _logger.LogInformation($"Published event ActivityCreated(id={command.Id})");
            }
            catch (EfforteoException exception)
            {
                await _busClient.PublishAsync(new CreateActivityRejected(command.Id, exception.Code, exception.Message));
                _logger.LogError($"Published event CreateActivityRejected(id={command.Id}, code={exception.Code}, message={exception.Message})");
            }
            catch (Exception exception)
            {
                await _busClient.PublishAsync(new CreateActivityRejected(command.Id, "error", exception.Message));
                _logger.LogError($"Published event CreateActivityRejected(id={command.Id}, code=unknown, message={exception.Message})");
            }
        }
    }
}