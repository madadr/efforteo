using System;
using System.Threading.Tasks;
using Efforteo.Common.Commands;
using Efforteo.Common.Events;
using Efforteo.Common.Exceptions;
using Efforteo.Services.Authentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RawRabbit;

namespace Efforteo.Services.Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger _logger;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IBusClient _busClient;

        private Guid UserId =>
            string.IsNullOrWhiteSpace(User?.Identity?.Name) ? Guid.Empty : Guid.Parse(User.Identity.Name);

        public AuthenticationController(IUserService userService, ILogger<AuthenticationController> logger,
            ICommandDispatcher commandDispatcher, IBusClient busClient)
        {
            _userService = userService;
            _logger = logger;
            _commandDispatcher = commandDispatcher;
            _busClient = busClient;
        }

        [HttpGet("id")]
        [Authorize]
        public IActionResult GetId()
        {
            _logger.LogInformation($"AuthenticationController::GetId: UserId = {UserId.ToString()}");

            return new JsonResult(new
            {
                id = UserId.ToString()
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUser command)
        {
            _logger.LogInformation(
                $"AuthenticationController::Register: command={JsonConvert.SerializeObject(command)}");

            await _commandDispatcher.DispatchAsync(command);

            return NoContent();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthenticateUser command)
        {
            _logger.LogInformation($"AuthenticationController::Login: command={JsonConvert.SerializeObject(command)}");

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

        [HttpPut("password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePassword command)
        {
            _logger.LogInformation(
                $"AuthenticationController::ChangePassword: command={JsonConvert.SerializeObject(command)}, UserId={UserId}");

            // Securing command by swapping request UserId with JWT UserId
            command.UserId = UserId;
            await _commandDispatcher.DispatchAsync(command);

            return Ok();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RemoveUser()
        {
            _logger.LogInformation($"AuthenticationController::RemoveUser id={UserId}");
            await _commandDispatcher.DispatchAsync(new RemoveUser() {UserId = UserId});

            return Ok();
        }
    }
}