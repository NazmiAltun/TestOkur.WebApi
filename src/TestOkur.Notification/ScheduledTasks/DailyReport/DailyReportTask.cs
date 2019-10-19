namespace TestOkur.Notification.ScheduledTasks.DailyReport
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Logging;
    using System.Linq;
    using System.Threading.Tasks;
    using TestOkur.Notification.Extensions;
    using TestOkur.Notification.Infrastructure;
    using TestOkur.Notification.Infrastructure.Clients;
    using TestOkur.Notification.Infrastructure.Data;
    using TestOkur.Notification.Models;

    internal class DailyReportTask : IDailyReportTask
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly INotificationFacade _notificationFacade;
        private readonly IStatsRepository _statsRepository;
        private readonly IWebApiClient _webApiClient;
        private readonly IOAuthClient _oAuthClient;
        private readonly IReportClient _reportClient;
        private readonly ILogger<DailyReportTask> _logger;

        public DailyReportTask(
            INotificationFacade notificationFacade,
            IWebHostEnvironment hostingEnvironment,
            ILogger<DailyReportTask> logger,
            IWebApiClient webApiClient,
            IOAuthClient oAuthClient,
            IReportClient reportClient,
            IStatsRepository statsRepository)
        {
            _notificationFacade = notificationFacade;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            _webApiClient = webApiClient;
            _oAuthClient = oAuthClient;
            _reportClient = reportClient;
            _statsRepository = statsRepository;
        }

        public async Task SendAsync()
        {
            if (_hostingEnvironment.IsProd())
            {
                await _notificationFacade.SendEmailToSystemAdminsAsync(
                    await GetDailyReportAsync(),
                    Template.DailyReportEmailAdmin);
            }
            else
            {
                _logger.LogWarning($"Daily Report on {_hostingEnvironment.EnvironmentName} env.");
            }
        }

        private async Task<DailyReportModel> GetDailyReportAsync()
        {
            var apiUsers = await _webApiClient.GetUsersAsync();

            return new DailyReportModel()
            {
                Statistics = await _webApiClient.GetStatisticsAsync(),
                ReportStatistics = await _reportClient.GetStatisticsAsync(),
                NotificationStatistics = await _statsRepository.GetStatisticsAsync(),
                IdentityStatistics = await _oAuthClient.GetDailyStatsAsync(),
            };
        }
    }
}
