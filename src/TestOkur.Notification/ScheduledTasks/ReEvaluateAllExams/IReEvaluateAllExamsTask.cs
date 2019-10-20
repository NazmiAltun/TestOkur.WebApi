namespace TestOkur.Notification.ScheduledTasks.ReEvaluateAllExams
{
    using System.Threading.Tasks;

    public interface IReEvaluateAllExamsTask : IScheduledTask
    {
        Task SendRequestAsync();
    }
}