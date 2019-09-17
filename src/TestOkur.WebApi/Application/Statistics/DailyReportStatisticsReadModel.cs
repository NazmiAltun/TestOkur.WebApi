namespace TestOkur.WebApi.Application.Statistics
{
    using System;

    public class DailyReportStatisticsReadModel
    {
        public DateTime Today { get; set; }

        public int LongestSMSDuration { get; set; }

        public int ShortestSMSDuration { get; set; }

        public int AverageSMSDuration { get; set; }

        public int TotalSuccessfulSMSCountInDay { get; set; }

        public int TotalFailedSMSCountInDay { get; set; }

        public int TotalSystemSMSCountInDay { get; set; }

        public int TotalUserSMSCountInDay { get; set; }

        public int TotalSMSCount { get; set; }

        public string TopSMSSenderEmailAddressInDay { get; set; }

        public int TopSMSSenderCountInDay { get; set; }

        public string TopSMSSenderEmailAddressForAllTime { get; internal set; }

        public int TopSMSSenderCountForAllTime { get; internal set; }

        public string TopLoginEmailForAllTime { get; internal set; }

        public int TopLoginCountForAllTime { get; internal set; }

        public int TotalIndividualLoginCountInDay { get; internal set; }

        public string ExpiredLicensesToday { get; internal set; }
    }
}
