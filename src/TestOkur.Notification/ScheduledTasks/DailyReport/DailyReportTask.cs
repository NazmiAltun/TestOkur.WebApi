namespace TestOkur.Notification.ScheduledTasks.DailyReport
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using TestOkur.Notification.Extensions;
    using TestOkur.Notification.Infrastructure;
    using TestOkur.Notification.Infrastructure.Clients;
    using TestOkur.Notification.Infrastructure.Data;
    using TestOkur.Notification.Models;

    internal class DailyReportTask : IDailyReportTask
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly INotificationFacade _notificationFacade;
        private readonly ISmsRepository _smsRepository;
        private readonly IWebApiClient _webApiClient;
        private readonly IOAuthClient _oAuthClient;
        private readonly IReportClient _reportClient;
        private readonly ILogger<DailyReportTask> _logger;

        public DailyReportTask(
            INotificationFacade notificationFacade,
            IHostingEnvironment hostingEnvironment,
            ILogger<DailyReportTask> logger,
            ISmsRepository smsRepository,
            IWebApiClient webApiClient,
            IOAuthClient oAuthClient,
            IReportClient reportClient)
        {
            _notificationFacade = notificationFacade;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            _smsRepository = smsRepository;
            _webApiClient = webApiClient;
            _oAuthClient = oAuthClient;
            _reportClient = reportClient;
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
            var todaysSmsList = await _smsRepository.GetTodaysSmsesAsync();
            var durations = todaysSmsList.Select(s => s.ResponseDateTimeUtc.Subtract(s.RequestDateTimeUtc).TotalMilliseconds);
            var topUserSmsStats = todaysSmsList.GroupBy(
                    x => x.UserId, (userId, smses) => new
                    {
                        UserId = userId,
                        TotalCredit = smses.Sum(x => x.Credit),
                    }).OrderByDescending(x => x.TotalCredit)
                .FirstOrDefault();
            var apiUsers = await _webApiClient.GetUsersAsync();
            var identityUsers = await _oAuthClient.GetUsersAsync();
            var expiredUsersEmails = identityUsers.Where(u => u.ExpiryDateUtc != null &&
                                     u.ExpiryDateUtc.Value.Date == DateTime.Today)
                .Select(u => u.Email);

            return new DailyReportModel()
            {
                Statistics = await _webApiClient.GetStatisticsAsync(),
                ReportStatistics = await _reportClient.GetStatisticsAsync(),
                TotalSuccessfulSMSCountInDay = todaysSmsList.Count(s => s.Status == SmsStatus.Successful),
                AverageSMSDuration = (int)durations.Average(),
                TotalSmsCredit = todaysSmsList.Sum(s => s.Credit),
                LongestSMSDuration = (int)durations.Max(),
                TotalUserSMSCountInDay = todaysSmsList.Count(s => s.UserId != default),
                TotalSystemSMSCountInDay = todaysSmsList.Count(s => s.UserId == default),
                TotalFailedSMSCountInDay = todaysSmsList.Count(s => s.Status == SmsStatus.Failed),
                TopSMSSenderCountInDay = topUserSmsStats?.TotalCredit ?? 0,
                TopSMSSenderEmailAddressInDay = apiUsers.FirstOrDefault(u => u.Id == topUserSmsStats?.UserId)?.Email,
                ExpiredLicensesToday = string.Join(", ", expiredUsersEmails),
                TotalIndividualLoginCountInDay = (await _oAuthClient.GetTodaysLogins()).Count(),
            };
        }
    }
}
