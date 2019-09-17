namespace TestOkur.Notification.ScheduledTasks
{
    using System;

    public class DailyReportModel
    {
        public DateTime Today => DateTime.Today;

        public int TotalSuccessfulSMSCountInDay { get; set; }

        public int TotalUserSMSCountInDay { get; set; }

        public int TotalSystemSMSCountInDay { get; set; }

        public int TotalFailedSMSCountInDay { get; set; }

        public int LongestSMSDuration { get; set; }

        public int ShortestSMSDuration { get; set; }

        public int AverageSMSDuration { get; set; }

        public string TopSMSSenderEmailAddressInDay { get; set; }

        public int TopSMSSenderCountInDay { get; set; }

        public int TopSMSSenderCountForAllTime { get; set; }

        public string TopSMSSenderEmailAddressForAllTime { get; set; }

        public int TotalIndividualLoginCountInDay { get; set; }

        public string ExpiredLicensesToday { get; set; }
    }
}
