namespace TestOkur.Notification.Infrastructure.Data
{
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TestOkur.Notification.Configuration;
    using TestOkur.Notification.Dtos;
    using TestOkur.Notification.Models;

    public class StatsRepository : IStatsRepository
    {
        private readonly TestOkurContext _context;

        public StatsRepository(ApplicationConfiguration configuration)
        {
            _context = new TestOkurContext(configuration);
        }

        public async Task<NotificationStatisticsDto> GetStatisticsAsync()
        {
            var todaysSmsList = await GetTodaysSmsesAsync();
            var durations = todaysSmsList.Select(s => s.ResponseDateTimeUtc.Subtract(s.RequestDateTimeUtc).TotalMilliseconds);
            var topUserSmsStats = todaysSmsList.GroupBy(
                    x => x.UserId, (userId, smses) => new
                    {
                        UserId = userId,
                        TotalCredit = smses.Sum(x => x.Credit),
                    }).OrderByDescending(x => x.TotalCredit)
                .FirstOrDefault();

            return new NotificationStatisticsDto
            {
                TotalSuccessfulSmsCountInDay = todaysSmsList.Count(s => s.Status == SmsStatus.Successful),
                AverageSmsDuration = (int)(durations.Any() ? durations.Average() : 0),
                TotalSmsCredit = todaysSmsList.Sum(s => s.Credit),
                LongestSmsDuration = (int)(durations.Any() ? durations.Max() : 0),
                TotalUserSmsCountInDay = todaysSmsList.Count(s => s.UserId != default),
                TotalSystemSmsCountInDay = todaysSmsList.Count(s => s.UserId == default),
                TotalFailedSmsCountInDay = todaysSmsList.Count(s => s.Status == SmsStatus.Failed),
                TopSmsSenderCreditInDay = topUserSmsStats?.TotalCredit ?? 0,
                TotalSmsCountAll = await _context.Smses.EstimatedDocumentCountAsync(),
                TotalSmsCreditsAll = await _context.Smses.AsQueryable()
                    .Where(s => s.UserId > 0).SumAsync(s => s.Credit),
            };
        }

        private async Task<IEnumerable<Sms>> GetTodaysSmsesAsync()
        {
            var filter = Builders<Sms>.Filter.Gte(x => x.CreatedOnDateTimeUtc, DateTime.UtcNow.Date);

            return await _context.Smses
                .Find(filter)
                .ToListAsync();
        }
    }
}
