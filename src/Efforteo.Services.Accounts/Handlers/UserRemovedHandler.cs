using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Efforteo.Common.Events;
using Efforteo.Common.Exceptions;
using Efforteo.Services.Accounts.Domain.DTO;
using Efforteo.Services.Accounts.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Efforteo.Services.Accounts.Handlers
{
    public class UserRemovedHandler : IEventHandler<UserRemoved>
    {
        private readonly IAccountService _accountService;
        private readonly ILogger _logger;

        public UserRemovedHandler(IAccountService accountService, ILogger<UserRemoved> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        public async Task HandleAsync(UserRemoved @event)
        {
            _logger.LogInformation("UserRemovedHandler: Removing account...");

            try
            {
                var account = await _accountService.GetAsync(@event.Id);
                if (account != null)
                {
                    await _accountService.RemoveAsync(@event.Id);
                    _logger.LogInformation("UserCreatedHandler: Account removed");
                }
            }
            catch (EfforteoException exception)
            {
                _logger.LogError(
                    $"Failed to remove account: {JsonConvert.SerializeObject(@event)}, code={exception.Code}, message={exception.Message}");
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    $"Failed to remove account: {JsonConvert.SerializeObject(@event)}, code=unknown, message={exception.Message})");
            }
        }
    }
}