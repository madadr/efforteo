using System;
using System.Threading.Tasks;
using Efforteo.Common.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RawRabbit;

namespace Efforteo.Services.Activities.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : Controller
    {
        private readonly IBusClient _busClient;
        private ILogger _logger;

        private Guid UserId =>
            string.IsNullOrWhiteSpace(User?.Identity?.Name) ? Guid.Empty : Guid.Parse(User.Identity.Name);

        public ActivitiesController(IBusClient busClient, ILogger<ActivitiesController> logger)
        {
            _busClient = busClient;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateActivity command)
        {
            _logger.LogError($"CreateActivity = {JsonConvert.SerializeObject(command)}");

            command.Id = Guid.NewGuid();
            command.CreatedAt = DateTime.UtcNow;
            await _busClient.PublishAsync(command);

            return Accepted($"activities/{command.Id}");
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Get()
        {
            _logger.LogError($"USER ID = {UserId.ToString()}");

            await Task.CompletedTask;
            return Content("Authorized access");
        }
    }
}