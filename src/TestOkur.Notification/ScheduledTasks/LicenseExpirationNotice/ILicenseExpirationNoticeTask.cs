namespace TestOkur.Notification.ScheduledTasks.LicenseExpirationNotice
{
    using System.Threading.Tasks;

    internal interface ILicenseExpirationNoticeTask
    {
        Task NotifyUsersAsync();
    }
}
