using System;
using System.Threading.Tasks;
using Efforteo.Common.Commands;
using Efforteo.Services.Stats.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RawRabbit;

namespace Efforteo.Services.Stats.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : Controller
    {
        private readonly IStatService _statService;
        private readonly ILogger _logger;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IBusClient _busClient;

        private Guid UserId =>
            string.IsNullOrWhiteSpace(User?.Identity?.Name) ? Guid.Empty : Guid.Parse(User.Identity.Name);

        public StatsController(IStatService statService, ILogger<StatsController> logger, ICommandDispatcher commandDispatcher,
            IBusClient busClient)
        {
            _statService = statService;
            _logger = logger;
            _commandDispatcher = commandDispatcher;
            _busClient = busClient;
        }

        [HttpGet("activity/{id}", Name="getStatById")]
        public async Task<IActionResult> GetStat(Guid id)
        {
            _logger.LogInformation($"StatsController::GetStat id={id}");

            var stat = await _statService.GetAsync(id);
            return new JsonResult(stat);
        }

        [HttpGet("total/{userId}", Name = "getTotalByUserId")]
        public async Task<IActionResult> GetTotalStats(Guid userId)
        {
            _logger.LogInformation($"StatsController::GetTotalStats userId={userId}");

            var stat = await _statService.GetTotalAsync(userId);
            if (stat == null)
            {
                return NoContent();
            }

            return new JsonResult(stat);
        }

        [HttpGet("period", Name = "getPeriodStats")]
        public async Task<IActionResult> GetPeriodStats(GetPeriodStats command)
        {
            _logger.LogInformation($"StatsController::GetPeriodStats command={JsonConvert.SerializeObject(command)}");

            var stat = await _statService.GetPeriodAsync(command.UserId, command.Days);
            if (stat == null)
            {
                return NoContent();
            }

            return new JsonResult(stat);
        }
    }
}
