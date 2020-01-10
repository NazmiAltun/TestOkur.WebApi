namespace TestOkur.Notification.Infrastructure.Clients
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TestOkur.Notification.Models;

    public interface IWebApiClient
    {
        Task<IEnumerable<UserModel>> GetUsersAsync();

        Task<StatisticsReadModel> GetStatisticsAsync();

        Task ReEvaluateAllExamsAsync();
    }
}