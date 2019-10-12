namespace TestOkur.Notification.Infrastructure.Clients
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TestOkur.Notification.Models;

    public interface IOAuthClient
    {
        Task<string> GetTokenAsync();

        Task<IEnumerable<IdentityUser>> GetUsersAsync();

        Task<IdentityStatisticsModel> GetDailyStatsAsync();
    }
}