using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Efforteo.Common.Commands;
using Efforteo.Common.Events;
using Efforteo.Common.Exceptions;
using Efforteo.Services.Authentication.Services;
using Microsoft.Extensions.Logging;
using RawRabbit;

namespace Efforteo.Services.Authentication.Handlers
{
    public class RemoveUserHandler : ICommandHandler<RemoveUser>
    {
        private readonly ILogger _logger;
        private readonly IBusClient _busClient;
        private readonly IUserService _userService;

        public RemoveUserHandler(IBusClient busClient, IUserService userService, ILogger<CreateUserHandler> logger)
        {
            _busClient = busClient;
            _userService = userService;
            _logger = logger;
        }

        public async Task HandleAsync(RemoveUser command)
        {
            _logger.LogInformation($"Removing user... Id={command.UserId}");

            try
            {
                var user = await _userService.GetAsync(command.UserId);

                await _userService.RemoveAsync(command.UserId);

                await _busClient.PublishAsync(new UserRemoved(user.Id, user.Email));
                _logger.LogInformation($"User removed. ID={user.Id}, Email='{user.Email}'");
            }
            catch (EfforteoException ex)
            {
                _logger.LogError(ex, ex.Message);
                await _busClient.PublishAsync(new RemoveUserRejected(command.UserId, ex.Code, ex.Message));

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await _busClient.PublishAsync(new RemoveUserRejected(command.UserId, "error", ex.Message));

                throw;
            }
        }
    }
}