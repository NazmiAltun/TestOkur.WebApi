namespace TestOkur.Notification.Infrastructure.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TestOkur.Notification.Models;

    public interface ISmsRepository
    {
        Task AddManyAsync(IEnumerable<Sms> list);

        Task AddAsync(Sms sms);

        Task UpdateSmsAsync(Sms sms);

        Task<IEnumerable<Sms>> GetTodaysSmsesAsync();
    }
}
