namespace TestOkur.Report.Infrastructure.Repositories
{
    using System.Threading.Tasks;
    using MongoDB.Driver;
    using TestOkur.Report.Configuration;
    using TestOkur.Report.Domain.Statistics;

    public class ExamStatisticsRepository : IExamStatisticsRepository
    {
        private readonly TestOkurContext _context;

        public ExamStatisticsRepository(ReportConfiguration configuration)
        {
            _context = new TestOkurContext(configuration);
        }

        public Task AddOrUpdateAsync(ExamStatistics examStatistics)
        {
            if (examStatistics.ExamId == default)
            {
                return Task.CompletedTask;
            }

            var filter = Builders<ExamStatistics>.Filter.Eq(x => x.ExamId, examStatistics.ExamId);
            return _context.ExamStatistics.ReplaceOneAsync(
                filter,
                examStatistics,
                new ReplaceOptions
                {
                    IsUpsert = true,
                });
        }

        public Task<ExamStatistics> GetAsync(int examId)
        {
            return _context.ExamStatistics
                .Find(_ => _.ExamId == examId)
                .SingleOrDefaultAsync();
        }
    }
}
