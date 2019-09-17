namespace TestOkur.Notification.ScheduledTasks
{
    using System.Threading.Tasks;

    internal interface ISendLicenseExpirationNotice
    {
        Task NotifyUsersAsync();
    }
}
