namespace TestOkur.Notification.ScheduledTasks.LicenseExpirationNotice
{
    using System.Threading.Tasks;

    internal interface ILicenseExpirationNoticeTask : IScheduledTask
    {
        Task NotifyUsersAsync();
    }
}
