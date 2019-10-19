namespace TestOkur.Notification.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.Notification.Dtos;
    using TestOkur.Notification.Infrastructure.Data;

    [Route("api/v1/statistics")]
    [Authorize(AuthorizationPolicies.Admin)]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatsRepository _statsRepository;

        public StatisticsController(IStatsRepository statsRepository)
        {
            _statsRepository = statsRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(NotificationStatisticsDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _statsRepository.GetStatisticsAsync());
        }
    }
}
