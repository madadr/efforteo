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
    public class RemoveActivityHandler : ICommandHandler<RemoveActivity>
    {
        private readonly IBusClient _busClient;
        private readonly IActivityService _activityService;
        private readonly ILogger _logger;

        public RemoveActivityHandler(IBusClient busClient, IActivityService activityService, ILogger<CreateActivityHandler> logger)
        {
            _busClient = busClient;
            _activityService = activityService;
            _logger = logger;
        }

        public async Task HandleAsync(RemoveActivity command)
        {
            _logger.LogInformation($"Removing activity... {command}");

            try
            {
                var activity = await _activityService.GetAsync(command.Id);
                if (activity.UserId == command.UserId)
                {
                    await _activityService.RemoveAsync(command.Id);
                    await _busClient.PublishAsync(new ActivityRemoved(command.UserId, command.Id));
                    _logger.LogInformation($"Published event ActivityRemoved(id={command.Id})");

                    return;
                }
                throw  new EfforteoException("activity_not_users", "Activity doesn't belong to this user.");
            }
            catch (EfforteoException exception)
            {
                await _busClient.PublishAsync(new RemoveActivityRejected(command.Id, exception.Code, exception.Message));
                _logger.LogError($"Published event RemoveActivityRejected(id={command.Id}, code={exception.Code}, message={exception.Message})");

                throw;
            }
            catch (Exception exception)
            {
                await _busClient.PublishAsync(new RemoveActivityRejected(command.Id, "error", exception.Message));
                _logger.LogError($"Published event RemoveActivityRejected(id={command.Id}, code=unknown, message={exception.Message})");

                throw;
            }
        }
    }
}