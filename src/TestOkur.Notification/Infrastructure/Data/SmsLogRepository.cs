namespace TestOkur.Notification.Infrastructure.Data
{
    using System.Threading.Tasks;
    using TestOkur.Notification.Configuration;
    using TestOkur.Notification.Dtos;

    public class SmsLogRepository : ISmsLogRepository
    {
        private readonly TestOkurContext _context;

        public SmsLogRepository(ApplicationConfiguration configuration)
        {
            _context = new TestOkurContext(configuration);
        }

        public Task LogAsync(SmsLog log) => _context.SmsLogs.InsertOneAsync(log);
    }
}