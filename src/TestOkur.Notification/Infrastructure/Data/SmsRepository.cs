namespace TestOkur.Notification.Infrastructure.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MongoDB.Driver;
    using TestOkur.Notification.Configuration;
    using TestOkur.Notification.Models;

    internal class SmsRepository : ISmsRepository
	{
		private readonly TestOkurContext _context;

		public SmsRepository(ApplicationConfiguration configuration)
		{
			_context = new TestOkurContext(configuration);
		}

		public async Task AddManyAsync(IEnumerable<Sms> list)
		{
			await _context.Smses.InsertManyAsync(list);
		}

		public async Task AddAsync(Sms sms)
		{
			await _context.Smses.InsertOneAsync(sms);
		}

		public async Task UpdateSmsAsync(Sms sms)
		{
			var filter = Builders<Sms>.Filter.Eq(s => s.Id, sms.Id);
			await _context.Smses.ReplaceOneAsync(filter, sms);
		}
	}
}
