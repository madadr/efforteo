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
    public class UserAuthenticatedHandler : IEventHandler<UserAuthenticated>
    {
        private readonly IAccountService _accountService;
        private readonly ILogger _logger;

        public UserAuthenticatedHandler(IAccountService accountService, ILogger<UserAuthenticated> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        public async Task HandleAsync(UserAuthenticated @event)
        {
            _logger.LogInformation("UserAuthenticatedHandler: Updating account last logged in...");

            try
            {
                var account = await _accountService.GetAsync(@event.Email);

                if (account != null)
                {
                    await _accountService.UpdateLoggedInAsync(account.Id);
                    _logger.LogInformation("UserAuthenticatedHandler: Account last logged in updated");
                }
            }
            catch (EfforteoException exception)
            {
                _logger.LogError(
                    $"Failed to update last logged in for account: {JsonConvert.SerializeObject(@event)}, code={exception.Code}, message={exception.Message}");
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    $"Failed to update last logged in for account: {JsonConvert.SerializeObject(@event)}, code=unknown, message={exception.Message})");
            }
        }
    }
}