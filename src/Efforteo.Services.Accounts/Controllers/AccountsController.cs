using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Efforteo.Common.Commands;
using Efforteo.Common.Events;
using Efforteo.Common.Exceptions;
using Efforteo.Services.Accounts.Services;
using Efforteo.Services.Accounts.Handlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Newtonsoft.Json;
using RawRabbit;
using System.Security.Claims;

namespace Efforteo.Services.Accounts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger _logger;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IBusClient _busClient;

        private Guid UserId =>
            string.IsNullOrWhiteSpace(User?.Identity?.Name) ? Guid.Empty : Guid.Parse(User.Identity.Name);

        public AccountsController(IUserService userService, ILogger<AccountsController> logger, ICommandDispatcher commandDispatcher , IBusClient busClient)
        {
            _userService = userService;
            _logger = logger;
            _commandDispatcher = commandDispatcher;
            _busClient = busClient;
        }

        [HttpGet("id")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetId()
        {
            return Content($"UserId: ${UserId}");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUser command)
        {
            _logger.LogTrace($"AccountsController::Register: command={JsonConvert.SerializeObject(command)}");

            command.Id = new Guid();
            await _commandDispatcher.DispatchAsync(command);

            return NoContent();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthenticateUser command)
        {
            _logger.LogTrace($"AccountsController::Register: command={JsonConvert.SerializeObject(command)}");

            try
            {
                var token = await _userService.LoginAsync(command.Email, command.Password);
                await _busClient.PublishAsync(new UserAuthenticated(command.Email));
                return Json(token);
            }
            catch (EfforteoException ex)
            {
                _logger.LogError(ex, ex.Message);
                await _busClient.PublishAsync(new AuthenticateUserRejected(command.Email, ex.Code, ex.Message));
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await _busClient.PublishAsync(new AuthenticateUserRejected(command.Email, "error", ex.Message));
                throw;
            }
        }

        [HttpPost("password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePassword command)
        {
            _logger.LogTrace($"AccountsController::ChangePassword: command={JsonConvert.SerializeObject(command)}");

            // Securing command by swapping request UserId with JWT UserId
            command.UserId = UserId;
            await _commandDispatcher.DispatchAsync(command);

            return Ok();
        }
    }
}