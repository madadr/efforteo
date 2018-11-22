using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Efforteo.Common.Commands;
using Efforteo.Common.Events;
using Efforteo.Common.Exceptions;
using Efforteo.Services.Accounts.Services;
using Microsoft.Extensions.Logging;
using RawRabbit;

namespace Efforteo.Services.Accounts.Handlers
{
    public class ChangePasswordHandler : ICommandHandler<ChangePassword>
    {
        private readonly ILogger _logger;
        private readonly IBusClient _busClient;
        private readonly IUserService _userService;

        public ChangePasswordHandler(IBusClient busClient, IUserService userService, ILogger<CreateUserHandler> logger)
        {
            _busClient = busClient;
            _userService = userService;
            _logger = logger;
        }

        public async Task HandleAsync(ChangePassword command)
        {
            _logger.LogInformation($"Changing password: UserId='{command.UserId}'");

            try
            {
                await _userService.ChangePassword(command.UserId, command.OldPassword, command.NewPassword);
                await _busClient.PublishAsync(new PasswordChanged(command.UserId));
                _logger.LogInformation($"Password changed: UserId='{command.UserId}'");
            }
            catch (EfforteoException ex)
            {
                _logger.LogError(ex, ex.Message);
                await _busClient.PublishAsync(new ChangePasswordRejected(command.UserId, ex.Code, ex.Message));
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await _busClient.PublishAsync(new ChangePasswordRejected(command.UserId, "error", ex.Message));
                throw;
            }
        }
    }
}