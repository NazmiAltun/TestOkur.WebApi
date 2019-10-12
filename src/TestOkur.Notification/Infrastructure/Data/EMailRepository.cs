namespace TestOkur.Notification.Infrastructure.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MongoDB.Driver;
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

        public async Task<List<EMail>> GetEmailsAsync(DateTime from, DateTime to)
        {
            var filter = Builders<EMail>.Filter.Gte(e => e.SentOnUtc, from.ToUniversalTime());
            filter &= Builders<EMail>.Filter.Lte(e => e.SentOnUtc, to.ToUniversalTime());

            return await _context.Emails.Find(filter).ToListAsync();
        }
    }
}
