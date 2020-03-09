namespace TestOkur.Notification.Infrastructure.Clients
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TestOkur.Notification.Models;

    public interface IReportClient
    {
        Task<ReportStatisticsModel> GetStatisticsAsync();

        Task<IEnumerable<ExamStatistics>> GetExamStatisticsAsync(IEnumerable<int> examIds);
    }
}