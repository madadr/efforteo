using System;
using System.Net;
using System.Threading.Tasks;
using Efforteo.Common.Commands;
using Efforteo.Services.Accounts.Services;
using Efforteo.Services.Accounts.Handlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Newtonsoft.Json;
using RawRabbit;

namespace Efforteo.Services.Accounts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        private Guid UserId =>
            string.IsNullOrWhiteSpace(User?.Identity?.Name) ? Guid.Empty : Guid.Parse(User.Identity.Name);

        public AccountsController(IUserService userService, ILogger<AccountsController> logger, ICommandDispatcher commandDispatcher)
        {
            _userService = userService;
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpGet("id")]
        [Authorize]
        public IActionResult GetId()
        {
            return Content($"UserId: ${UserId}");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUser command)
        {
            _logger.LogTrace($"Register command = {JsonConvert.SerializeObject(command)}");

            command.Id = new Guid();

            await _commandDispatcher.DispatchAsync(command);

            return NoContent();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthenticateUser command)
        {
            _logger.LogTrace($"Login command = {JsonConvert.SerializeObject(command)}");

            return Json(await _userService.LoginAsync(command.Email, command.Password));
        }


//        [HttpPost("password")]
//        public async Task<IActionResult> ChangePassword(ChangePassword command)
//        {
//            _logger.LogTrace($"ChangePassword command = {JsonConvert.SerializeObject(command)}");
        // TODO: check if for UserId current password is valid, then check if new password is valid
//            await _commandDispatcher.DispatchAsync(command);

//            return Accepted();
//        }
    }
}