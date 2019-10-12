namespace TestOkur.Notification.Models
{
    public class IdentityStatisticsModel
    {
        public string ExpiredUsersToday { get; set; }

        public int TotalIndividualLoginCountInDay { get; set; }

        public int TotalUserCount { get; set; }

        public int TotalActiveUserCount { get; set; }

        public int NewUserActivatedCountToday { get; set; }

        public int SubscriptionExtendedCountToday { get; set; }
    }
}
