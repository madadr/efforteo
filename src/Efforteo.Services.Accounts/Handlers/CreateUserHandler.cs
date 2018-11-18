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
    public class CreateUserHandler : ICommandHandler<CreateUser>
    {
        private readonly ILogger _logger;
        private readonly IBusClient _busClient;
        private readonly IUserService _userService;

        public CreateUserHandler(IBusClient busClient, IUserService userService, ILogger<CreateUserHandler> logger)
        {
            _busClient = busClient;
            _userService = userService;
            _logger = logger;
        }

        public async Task HandleAsync(CreateUser command)
        {
            _logger.LogInformation($"Creating user... Email='{command.Email}', name='{command.Name}'.");

            try
            {
                await _userService.RegisterAsync(command.Email, command.Password, command.Name);
                await _busClient.PublishAsync(new UserCreated(command.Email, command.Name));
                _logger.LogInformation($"User created. Email='{command.Email}', name='{command.Name}'.");
            }
            catch (EfforteoException ex)
            {
                _logger.LogError(ex, ex.Message);
                await _busClient.PublishAsync(new CreateUserRejected(command.Email, ex.Code, ex.Message));
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await _busClient.PublishAsync(new CreateUserRejected(command.Email, "error", ex.Message));
                throw;
            }
        }
    }
}