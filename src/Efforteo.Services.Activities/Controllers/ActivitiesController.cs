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
    public class ActivitiesController : ControllerBase
    {
        private readonly IBusClient _busClient;
        private ILogger _logger;

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
        public IActionResult Get()
        {
            return Content("Authorized access");
        }
    }
}