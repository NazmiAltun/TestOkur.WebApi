﻿namespace TestOkur.Notification.Infrastructure.Data
{
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MongoDB.Bson;
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

        public async Task<IEnumerable<Sms>> GetTodaysSmsesAsync()
        {
            var filter = Builders<Sms>.Filter.Gte(x => x.CreatedOnDateTimeUtc, DateTime.Today);

            return await _context.Smses
                .Find(filter)
                .ToListAsync();
        }
    }
}
