namespace TestOkur.Notification.ScheduledTasks
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Logging;
    using TestOkur.Notification.Extensions;
    using TestOkur.Notification.Infrastructure;
    using TestOkur.Notification.Models;

    internal class DailyReport : IDailyReport
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly INotificationFacade _notificationFacade;
        private readonly ILogger<DailyReport> _logger;

        public DailyReport(INotificationFacade notificationFacade, IHostingEnvironment hostingEnvironment, ILogger<DailyReport> logger)
        {
            _notificationFacade = notificationFacade;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        public async Task SendAsync()
        {
            if (_hostingEnvironment.IsProd())
            {
                await _notificationFacade.SendEmailToSystemAdminsAsync(
                    new DailyReportModel(),
                    Template.DailyReportEmailAdmin);
            }
            else
            {
                _logger.LogWarning($"Daily Report on {_hostingEnvironment.EnvironmentName} env.");
            }
        }
    }
}
