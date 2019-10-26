namespace TestOkur.Notification.Infrastructure.Data
{
    using System.Threading.Tasks;
    using TestOkur.Notification.Dtos;

    public interface ISmsLogRepository
    {
        Task LogAsync(SmsLog log);
    }
}