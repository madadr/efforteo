using System.Threading.Tasks;
using Efforteo.Common.Commands;
using Efforteo.Services.Identity.Services;
using Microsoft.AspNetCore.Mvc;
using RawRabbit;

namespace Efforteo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthenticateUser command)
            => new JsonResult(await _userService.LoginAsync(command.Email, command.Password));
    }
}