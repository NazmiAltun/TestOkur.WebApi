namespace TestOkur.Report.Infrastructure.Repositories
{
    using System.Threading.Tasks;
    using TestOkur.Report.Models;

    public interface IReportRequestRepository
    {
        Task AddAsync(ReportRequest reportRequest);

        Task<ReportStatisticsModel> GetStatisticsAsync();
    }
}
