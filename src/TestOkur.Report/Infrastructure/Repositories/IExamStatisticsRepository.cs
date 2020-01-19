namespace TestOkur.Report.Infrastructure.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TestOkur.Report.Domain.Statistics;

    public interface IExamStatisticsRepository
    {
        Task AddOrUpdateAsync(ExamStatistics examStatistics);

        Task<ExamStatistics> GetAsync(int examId);

        Task<List<ExamStatistics>> GetListAsync(IEnumerable<int> examIds);
    }
}