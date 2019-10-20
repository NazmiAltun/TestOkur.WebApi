namespace TestOkur.Notification.ScheduledTasks.DailyReport
{
    using System.Threading.Tasks;

    internal interface IDailyReportTask : IScheduledTask
    {
        Task SendAsync();
    }
}
