namespace TestOkur.Notification.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.Notification.Dtos;
    using TestOkur.Notification.Infrastructure.Clients;
    using TestOkur.Notification.Infrastructure.Data;

    [Route("api/v1/statistics")]
    [Authorize(AuthorizationPolicies.Admin)]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatsRepository _statsRepository;
        private readonly IWebApiClient _webApiClient;

        public StatisticsController(IStatsRepository statsRepository, IWebApiClient webApiClient)
        {
            _statsRepository = statsRepository;
            _webApiClient = webApiClient;
        }

        [HttpGet]
        [ProducesResponseType(typeof(NotificationStatisticsDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
        {
            var users = await _webApiClient.GetUsersAsync();
            var stats = await _statsRepository.GetStatisticsAsync();
            stats.TopSmsSenderEmailInDay = users.FirstOrDefault(u => u.Id == stats.TopSmsSenderIdInDay)
                ?.Email;

            return Ok(stats);
        }
    }
}
