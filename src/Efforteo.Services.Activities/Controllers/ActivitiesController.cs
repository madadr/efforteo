using System;
using System.Linq;
using System.Threading.Tasks;
using Efforteo.Common.Commands;
using Efforteo.Services.Activities.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RawRabbit;

namespace Efforteo.Services.Activities.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : Controller
    {
        private readonly IBusClient _busClient;
        private readonly ILogger _logger;
        private readonly IActivityService _activityService;
        private readonly ICommandDispatcher _commandDispatcher;

        private Guid UserId =>
            string.IsNullOrWhiteSpace(User?.Identity?.Name) ? Guid.Empty : Guid.Parse(User.Identity.Name);

        public ActivitiesController(IBusClient busClient, ILogger<ActivitiesController> logger, IActivityService activityService, ICommandDispatcher commandDispatcher)
        {
            _busClient = busClient;
            _logger = logger;
            _activityService = activityService;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateActivity command)
        {
            _logger.LogInformation($"ActivitiesController::CreateActivity = {JsonConvert.SerializeObject(command)}");

            command.Id = Guid.NewGuid();
            command.UserId = UserId;
            command.CreatedAt = DateTime.UtcNow;
            await _busClient.PublishAsync(command);

            return Accepted($"activities/{command.Id}");
        }

        [HttpGet("{id}", Name ="GetActivityById")]
        public async Task<IActionResult> GetActivity(Guid id)
        {
            _logger.LogInformation($"ActivitiesController::GetActivity: id={id.ToString()}");

            var activity = await _activityService.GetAsync(id);
            return new JsonResult(activity);
        }

        [Route("user")]
        [HttpGet("{id}", Name = "GetActivityByUserId")]
        public async Task<IActionResult> GetUserActivity(Guid id)
        {
            _logger.LogInformation($"ActivitiesController::GetUserActivity: userId={id.ToString()}");

            var activities = await _activityService.GetUserActivitiesAsync(id);
            return new JsonResult(activities.Select(activity => new
            {
                activity.Id,
                activity.UserId,
                activity.Name,
                activity.Category
            }));
        }

        [Route("user")]
        [HttpGet(Name = "GetThisUserActivity")]
        public async Task<IActionResult> GetThisUserActivity()
        {
            return await GetUserActivity(UserId);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveActivity(Guid id)
        {
            await _commandDispatcher.DispatchAsync(new RemoveActivity()
            {
                Id = id,
                UserId = UserId
            });

            return Ok();
        }
    }
}