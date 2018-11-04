using System.Threading.Tasks;
using Efforteo.Common.Commands;
using Microsoft.AspNetCore.Mvc;
using RawRabbit;

namespace Efforteo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IBusClient _busClient;

        public UsersController(IBusClient busClient)
        {
            _busClient = busClient;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Post(CreateAccount command)
        {
            await _busClient.PublishAsync(command);

            return Accepted();
        }
    }
}