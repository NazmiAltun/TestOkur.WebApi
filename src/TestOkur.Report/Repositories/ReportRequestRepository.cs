namespace TestOkur.Report.Repositories
{
    using System.Threading.Tasks;
    using TestOkur.Report.Configuration;
    using TestOkur.Report.Infrastructure;
    using TestOkur.Report.Models;

    public class ReportRequestRepository : IReportRequestRepository
    {
        private readonly TestOkurContext _context;

        public ReportRequestRepository(ReportConfiguration configuration)
        {
            _context = new TestOkurContext(configuration);
        }

        public async Task AddAsync(ReportRequest reportRequest)
        {
            await _context.ReportRequests.InsertOneAsync(reportRequest);
        }
    }
}