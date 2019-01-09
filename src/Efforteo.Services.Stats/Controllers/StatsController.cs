using System;
using System.Threading.Tasks;
using Efforteo.Common.Commands;
using Efforteo.Common.Exceptions;
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

        private Guid UserId =>
            string.IsNullOrWhiteSpace(User?.Identity?.Name) ? Guid.Empty : Guid.Parse(User.Identity.Name);

        public StatsController(IStatService statService, ILogger<StatsController> logger)
        {
            _statService = statService;
            _logger = logger;
        }

        [HttpGet("activity/{id}", Name = "getStatById")]
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

        [HttpGet("period/{userId}/{days}", Name = "getPeriodStats")]
        public async Task<IActionResult> GetPeriodStats(Guid userId, int days)
        {
            _logger.LogInformation($"StatsController::GetPeriodStats userId={userId}, days={days}");
            if (days <= 0)
            {
                throw new EfforteoException("empty_days", "Cannot get period stats. Invalid days period.");
            }

            var stat = await _statService.GetPeriodAsync(userId, days);
            if (stat == null)
            {
                return NoContent();
            }

            return new JsonResult(stat);
        }


        [HttpGet("detailed/{userId}", Name = "GetDetailedStats")]
        public async Task<IActionResult> GetDetailedStats(Guid userId)
        {
            _logger.LogInformation($"StatsController::GetDetailedStats userId={userId}");
            var stat = await _statService.GetDetailedStats(userId);
            if (stat == null)
            {
                return NoContent();
            }

            return new JsonResult(stat);
        }
    }
}