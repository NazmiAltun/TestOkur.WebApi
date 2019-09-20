namespace TestOkur.Notification.ScheduledTasks.DailyReport
{
    using System;
    using TestOkur.Notification.Models;

    public class DailyReportModel
    {
        public DateTime Today => DateTime.Today;

        public int TotalSuccessfulSMSCountInDay { get; set; }

        public int TotalSmsCredit { get; set; }

        public int TotalUserSMSCountInDay { get; set; }

        public int TotalSystemSMSCountInDay { get; set; }

        public int TotalFailedSMSCountInDay { get; set; }

        public int LongestSMSDuration { get; set; }

        public int AverageSMSDuration { get; set; }

        public string TopSMSSenderEmailAddressInDay { get; set; }

        public int TopSMSSenderCountInDay { get; set; }

        public int TotalIndividualLoginCountInDay { get; set; }

        public string ExpiredLicensesToday { get; set; }

        public StatisticsReadModel Statistics { get; set; }
    }
}
