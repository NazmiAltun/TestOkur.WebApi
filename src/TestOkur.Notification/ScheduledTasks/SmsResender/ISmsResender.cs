namespace TestOkur.Notification.ScheduledTasks.SmsResender
{
    using System.Threading.Tasks;

    public interface ISmsResender : IScheduledTask
    {
        Task TryResendAsync();
    }
}
