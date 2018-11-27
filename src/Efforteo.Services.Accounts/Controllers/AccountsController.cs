using System;
using System.Linq;
using System.Threading.Tasks;
using Efforteo.Common.Commands;
using Efforteo.Services.Accounts.Domain.DTO;
using Efforteo.Services.Accounts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RawRabbit;

namespace Efforteo.Services.Accounts.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ILogger _logger;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IBusClient _busClient;

        private Guid UserId =>
            string.IsNullOrWhiteSpace(User?.Identity?.Name) ? Guid.Empty : Guid.Parse(User.Identity.Name);

        public AccountsController(IAccountService accountService, ILogger<AccountsController> logger, ICommandDispatcher commandDispatcher,
            IBusClient busClient)
        {
            _accountService = accountService;
            _logger = logger;
            _commandDispatcher = commandDispatcher;
            _busClient = busClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogTrace("AccountsController::GetAll");

            var accounts = await _accountService.GetAllAsync();
            return new JsonResult(accounts.Select(account => new
            {
                account.Id,
                account.Name,
                account.Location
            }));
        }

        // TODO: Consider detailed info only for friends
        [HttpGet("{id}", Name="getById")]
        public async Task<IActionResult> Get(Guid id)
        {
            _logger.LogTrace($"AccountsController::GetAccount id={id}");

            var account = await _accountService.GetAsync(id);
            return new JsonResult(account);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateAccount command)
        {
            _logger.LogTrace($"AccountsController::Update command={JsonConvert.SerializeObject(command)}, UserId={UserId}");

            command.UserId = UserId;

            await _accountService.UpdateAsync(new AccountDto()
            {
                Id = command.UserId,
                Name = command.Name,
                Email = command.Email,
                Location = command.Location,
                Weight = command.Weight,
                Birthday = command.Birthday
            });

            return Ok();
        }
    }
}