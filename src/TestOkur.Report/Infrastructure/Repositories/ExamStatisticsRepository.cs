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

        public async Task AddOrUpdateAsync(ExamStatistics examStatistics)
        {
            if (examStatistics.ExamId == default)
            {
                return;
            }

            var filter = Builders<ExamStatistics>.Filter.Eq(x => x.ExamId, examStatistics.ExamId);
            await _context.ExamStatistics.DeleteManyAsync(filter);
            await _context.ExamStatistics.InsertOneAsync(examStatistics);
        }

        public Task<ExamStatistics> GetAsync(int examId)
        {
            return _context.ExamStatistics
                .Find(_ => _.ExamId == examId)
                .SingleOrDefaultAsync();
        }
    }
}
