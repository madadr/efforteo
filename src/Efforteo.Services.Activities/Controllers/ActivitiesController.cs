using System;
using System.Linq;
using System.Threading.Tasks;
using Efforteo.Common.Commands;
using Efforteo.Common.Events;
using Efforteo.Common.Exceptions;
using Efforteo.Services.Activities.Domain.DTO;
using Efforteo.Services.Activities.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoreLinq;
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

        public ActivitiesController(IBusClient busClient, ILogger<ActivitiesController> logger,
            IActivityService activityService, ICommandDispatcher commandDispatcher)
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
            await _commandDispatcher.DispatchAsync(command);

            return Accepted($"api/activities/activity/{command.Id}");
        }
   
        [HttpGet(Name = "GetAllActivities")]
        public async Task<IActionResult> GetAllActivities()
        {
            _logger.LogInformation($"ActivitiesController::GetAllActivities");

            var activities = await _activityService.GetAllAsync();
            return new JsonResult(activities.Select(activity => new
            {
                activity.Id,
                activity.UserId,
                activity.Title,
                activity.Category,
                activity.Time,
                activity.Distance,
                activity.CreatedAt
            }).OrderBy(activity => activity.CreatedAt, direction: OrderByDirection.Descending));
        }

        [HttpGet("user/{id}", Name = "GetActivityByUserId")]
        public async Task<IActionResult> GetUserActivity(Guid id)
        {
            if (id == Guid.Empty)
            {
                id = UserId;
            }

            _logger.LogInformation($"ActivitiesController::GetUserActivity: userId={id.ToString()}");

            var activities = await _activityService.GetUserActivitiesAsync(id);
            return new JsonResult(activities.Select(activity => new
            {
                activity.Id,
                activity.UserId,
                activity.Title,
                activity.Category
            }));
        }

        [HttpGet("activity/{id}", Name = "GetActivityById")]
        public async Task<IActionResult> GetActivity(Guid id)
        {
            _logger.LogInformation($"ActivitiesController::GetActivity: id={id.ToString()}");

            var activity = await _activityService.GetAsync(id);
            return new JsonResult(activity);
        }


        [HttpPut("activity")]
        public async Task<IActionResult> Update(ActivityDto command)
        {
            _logger.LogInformation(
                $"ActivitiesController::Update command={JsonConvert.SerializeObject(command)}, UserId={UserId}");

            command.UserId = UserId;

            try
            {
                await _activityService.UpdateAsync(command);
                await _busClient.PublishAsync(new ActivityUpdated(command.Id, command.UserId, command.Category,
                    command.Title, command.Description, command.Time.GetValueOrDefault(-1),
                    command.Distance.GetValueOrDefault(-1), command.CreatedAt));
                return Ok();
            }
            catch (EfforteoException ex)
            {
                _logger.LogError(ex, ex.Message);
                await _busClient.PublishAsync(new ActivityUpdateRejected(command.Id, command.UserId, ex.Code,
                    ex.Message));
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await _busClient.PublishAsync(new ActivityUpdateRejected(command.Id, command.UserId, "error",
                    ex.Message));
                throw;
            }
        }

        [HttpDelete("activity/{id}")]
        public async Task<IActionResult> RemoveActivity(Guid id)
        {
            _logger.LogInformation($"ActivitiesController::RemoveActivity id={id}, UserId={UserId}");
            await _commandDispatcher.DispatchAsync(new RemoveActivity()
            {
                Id = id,
                UserId = UserId
            });

            return Ok();
        }
    }
}