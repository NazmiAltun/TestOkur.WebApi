namespace TestOkur.Notification.Infrastructure.Clients
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TestOkur.Notification.Models;

    public interface IWebApiClient
    {
        Task<IEnumerable<UserModel>> GetUsersAsync();

        Task<AppSettingReadModel> GetAppSettingAsync(string name);

        [Obsolete("This will be removed in future released. Better publish an event rather than making an http call to the api")]
        Task DeductSmsCreditsAsync(int userId, string smsBody);

        Task<StatisticsReadModel> GetStatisticsAsync();

        Task ReEvaluateAllExamsAsync();
    }
}