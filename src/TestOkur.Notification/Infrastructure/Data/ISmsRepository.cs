namespace TestOkur.Notification.Infrastructure.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TestOkur.Notification.Dtos;

    public interface ISmsRepository
    {
        Task AddManyAsync(IEnumerable<Sms> list);

        Task AddAsync(Sms sms);

        Task UpdateSmsAsync(Sms sms);

        Task<List<Sms>> GetTodaysSmsesAsync();

        Task<IEnumerable<Sms>> GetUserSmsesAsync(string userSubjectId);

        Task<List<Sms>> GetPendingOrFailedSmsesAsync();

        Task<List<SmsLog>> GetUserSmsLogsAsync(int userId);
    }
}
