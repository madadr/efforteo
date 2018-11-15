using System;
using System.Net;
using System.Threading.Tasks;
using Efforteo.Common.Commands;
using Efforteo.Services.Identity.Handlers;
using Efforteo.Services.Identity.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace Efforteo.Services.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger _logger;

        public AccountsController(IUserService userService, ILogger<AccountsController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUser command)
        {
            // TODO: Implement CommandDispatcher and use it!
            _logger.LogTrace($"Register command = {JsonConvert.SerializeObject(command)}");
            await _userService.RegisterAsync(command.Email, command.Password, command.Name);
            return Content("Success!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthenticateUser command)
        {
            _logger.LogTrace($"Login command = {JsonConvert.SerializeObject(command)}");

            return Json(await _userService.LoginAsync(command.Email, command.Password));
        }
    }
}