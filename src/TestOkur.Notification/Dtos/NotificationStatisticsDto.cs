namespace TestOkur.Notification.Dtos
{
    public class NotificationStatisticsDto
    {
        public int TotalSuccessfulSmsCountInDay { get; set; }

        public int TotalSmsCredit { get; set; }

        public int TotalUserSmsCountInDay { get; set; }

        public int TotalSystemSmsCountInDay { get; set; }

        public int TotalFailedSmsCountInDay { get; set; }

        public int LongestSmsDuration { get; set; }

        public int AverageSmsDuration { get; set; }

        public int TopSmsSenderCreditInDay { get; set; }

        public long TotalSmsCountAll { get; set; }

        public long TotalSmsCreditsAll { get; set; }
    }
}
