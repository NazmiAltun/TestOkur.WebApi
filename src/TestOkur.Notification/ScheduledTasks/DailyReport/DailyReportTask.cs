namespace TestOkur.Notification.ScheduledTasks.DailyReport
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Logging;
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
            var getUsersTask = _webApiClient.GetUsersAsync();
            var webApiStatisticsTask = _webApiClient.GetStatisticsAsync();
            var reportStatisticsTask = _reportClient.GetStatisticsAsync();
            var notificationStatisticsTask = _statsRepository.GetStatisticsAsync();
            var identityStatisticsTask = _oAuthClient.GetDailyStatsAsync();

            await Task.WhenAll(getUsersTask, webApiStatisticsTask, reportStatisticsTask, notificationStatisticsTask, identityStatisticsTask);
            var sharedExams = (await webApiStatisticsTask).SharedExams.ToDictionary(x => x.Id, x => x.Name);
            var examStatistics = await _reportClient.GetExamStatisticsAsync(sharedExams.Keys);

            var model = new DailyReportModel()
            {
                Statistics = await webApiStatisticsTask,
                ReportStatistics = await reportStatisticsTask,
                NotificationStatistics = await notificationStatisticsTask,
                IdentityStatistics = await identityStatisticsTask,
            };
            model.NotificationStatistics.TopSmsSenderEmailInDay =
                getUsersTask.Result.FirstOrDefault(u => u.Id == model.NotificationStatistics.TopSmsSenderIdInDay)
                ?.Email;
            model.SharedExamAttendance = examStatistics.Select(e =>
                new KeyValuePair<string, int>(sharedExams[e.ExamId], e.GeneralAttendanceCount));

            return model;
        }
    }
}
