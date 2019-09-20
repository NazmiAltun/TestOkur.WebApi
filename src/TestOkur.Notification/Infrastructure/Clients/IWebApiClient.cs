namespace TestOkur.Notification.Infrastructure.Clients
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TestOkur.Notification.Models;

    public interface IWebApiClient
    {
        Task<IEnumerable<UserModel>> GetUsersAsync();

        Task<AppSettingReadModel> GetAppSettingAsync(string name);

        Task DeductSmsCreditsAsync(int userId, string smsBody);

        Task<StatisticsReadModel> GetStatisticsAsync();
    }
}