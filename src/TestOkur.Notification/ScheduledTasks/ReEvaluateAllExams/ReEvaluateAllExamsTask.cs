namespace TestOkur.Notification.ScheduledTasks.ReEvaluateAllExams
{
    using System.Threading.Tasks;
    using TestOkur.Notification.Infrastructure.Clients;

    public class ReEvaluateAllExamsTask : IReEvaluateAllExamsTask
    {
        private readonly IWebApiClient _webApiClient;

        public ReEvaluateAllExamsTask(IWebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
        }

        public Task SendRequestAsync()
        {
            return _webApiClient.ReEvaluateAllExamsAsync();
        }
    }
}
