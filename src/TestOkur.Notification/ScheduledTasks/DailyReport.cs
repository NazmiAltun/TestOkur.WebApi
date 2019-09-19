namespace TestOkur.Notification.ScheduledTasks
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Logging;
    using TestOkur.Notification.Extensions;
    using TestOkur.Notification.Infrastructure;
    using TestOkur.Notification.Infrastructure.Clients;
    using TestOkur.Notification.Infrastructure.Data;
    using TestOkur.Notification.Models;

    internal class DailyReport : IDailyReport
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly INotificationFacade _notificationFacade;
        private readonly ISmsRepository _smsRepository;
        private readonly IWebApiClient _webApiClient;
        private readonly IOAuthClient _oAuthClient;
        private readonly ILogger<DailyReport> _logger;

        public DailyReport(
            INotificationFacade notificationFacade,
            IHostingEnvironment hostingEnvironment,
            ILogger<DailyReport> logger,
            ISmsRepository smsRepository,
            IWebApiClient webApiClient,
            IOAuthClient oAuthClient)
        {
            _notificationFacade = notificationFacade;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            _smsRepository = smsRepository;
            _webApiClient = webApiClient;
            _oAuthClient = oAuthClient;
        }

        public async Task SendAsync()
        {
            if (_hostingEnvironment.IsProd())
            {
                await _notificationFacade.SendEmailToSystemAdminsAsync(
                    GetDailyReportAsync(),
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
                .First();
            var apiUsers = await _webApiClient.GetUsersAsync();
            var identityUsers = await _oAuthClient.GetUsersAsync();
            var expiredUsersEmails = identityUsers.Where(u => u.ExpiryDateUtc != null &&
                                     u.ExpiryDateUtc.Value.Date == DateTime.Today)
                .Select(u => u.Email);

            return new DailyReportModel()
            {
                TotalSuccessfulSMSCountInDay = todaysSmsList.Count(s => s.Status == SmsStatus.Successful),
                AverageSMSDuration = (int)durations.Average(),
                TotalSmsCredit = todaysSmsList.Sum(s => s.Credit),
                LongestSMSDuration = (int)durations.Max(),
                TotalUserSMSCountInDay = todaysSmsList.Count(s => s.UserId != default),
                TotalSystemSMSCountInDay = todaysSmsList.Count(s => s.UserId == default),
                TotalFailedSMSCountInDay = todaysSmsList.Count(s => s.Status == SmsStatus.Failed),
                TopSMSSenderCountInDay = topUserSmsStats.TotalCredit,
                TopSMSSenderEmailAddressInDay = apiUsers.First(u => u.Id == topUserSmsStats.UserId).Email,
                ExpiredLicensesToday = string.Join(", ", expiredUsersEmails),
            };
        }
    }
}
