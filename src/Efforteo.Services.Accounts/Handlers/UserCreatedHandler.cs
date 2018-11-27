using System;
using System.Threading.Tasks;
using Efforteo.Common.Events;
using Efforteo.Common.Exceptions;
using Efforteo.Services.Accounts.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Efforteo.Services.Accounts.Handlers
{
    public class UserCreatedHandler : IEventHandler<UserCreated>
    {
        private readonly IAccountService _accountService;
        private readonly ILogger _logger;

        public UserCreatedHandler(IAccountService accountService, ILogger<UserCreatedHandler> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        public async Task HandleAsync(UserCreated @event)
        {
            _logger.LogInformation($"UserCreatedHandler: Creating account, e-mail='{@event.Email}'");

            try
            {
                await _accountService.AddAsync(@event.Id, @event.Email);
                _logger.LogInformation($"UserCreatedHandler: Account created, e-mail='{@event.Email}'");
            }
            catch (EfforteoException exception)
            {
                _logger.LogError($"Failed to create account for: {JsonConvert.SerializeObject(@event)}, code={exception.Code}, message={exception.Message}");
                return;
            }
            catch (Exception exception)
            {
                _logger.LogError($"Failed to create account for: {JsonConvert.SerializeObject(@event)}, code=unknown, message={exception.Message})");
                return;
            }

            try
            {
                var accountDto = await _accountService.GetAsync(@event.Id);
                accountDto.Name = @event.Name;
                await _accountService.UpdateAsync(accountDto);
                _logger.LogInformation($"UserCreatedHandler: Assigned username='{@event.Name}' to account e-mail='{@event.Email}'");
            }
            catch (EfforteoException exception)
            {
                _logger.LogError($"Failed to assign username for account: {JsonConvert.SerializeObject(@event)}, code={exception.Code}, message={exception.Message}");
            }
            catch (Exception exception)
            {
                _logger.LogError($"Failed to assign username for account: {JsonConvert.SerializeObject(@event)}, code=unknown, message={exception.Message})");
            }
        }
    }
}
