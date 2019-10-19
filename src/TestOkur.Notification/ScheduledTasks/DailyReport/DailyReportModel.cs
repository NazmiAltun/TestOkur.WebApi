namespace TestOkur.Notification.ScheduledTasks.DailyReport
{
    using System;
    using TestOkur.Notification.Dtos;
    using TestOkur.Notification.Models;

    public class DailyReportModel
    {
        public DateTime Today => DateTime.Today;

        public NotificationStatisticsDto NotificationStatistics { get; set; }

        public StatisticsReadModel Statistics { get; set; }

        public ReportStatisticsModel ReportStatistics { get; set; }

        public IdentityStatisticsModel IdentityStatistics { get; set; }
    }
}
