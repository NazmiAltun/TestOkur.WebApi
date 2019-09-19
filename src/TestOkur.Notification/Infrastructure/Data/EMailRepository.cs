namespace TestOkur.Notification.Infrastructure.Data
{
    using System.Threading.Tasks;
    using TestOkur.Notification.Configuration;
    using TestOkur.Notification.Models;

    public class EMailRepository : IEMailRepository
    {
        private readonly TestOkurContext _context;

        public EMailRepository(ApplicationConfiguration configuration)
        {
            _context = new TestOkurContext(configuration);
        }

        public async Task AddAsync(EMail email)
        {
            await _context.Emails.InsertOneAsync(email);
        }
    }
}
