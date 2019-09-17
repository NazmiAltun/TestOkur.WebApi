namespace TestOkur.Notification.ScheduledTasks
{
    using System.Threading.Tasks;

    internal interface IDailyReport
    {
        Task SendAsync();
    }
}
