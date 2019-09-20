namespace TestOkur.Report.Repositories
{
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

        public async Task<ReportStatisticsModel> GetStatisticsAsync()
        {
            var all = await _context.ReportRequests.Find(Builders<ReportRequest>.Filter.Empty).ToListAsync();

            return new ReportStatisticsModel
            {
                TotalCount = all.Count,
                TodayCount = all.Count(x => x.RequestDateTimeUtc.ToLocalTime().Date == DateTime.Today),
                CountByReportTypeTotal = CountGroupByAndJoin(all, x => x.ReportType),
                CountByExportTypeTotal = CountGroupByAndJoin(all, x => x.ExportType),
                CountByExportTypeToday = CountGroupByAndJoin(
                    all.Where(x => x.RequestDateTimeUtc.ToLocalTime().Date == DateTime.Today),
                    x => x.ExportType),
                CountByReportTypeToday = CountGroupByAndJoin(
                    all.Where(x => x.RequestDateTimeUtc.ToLocalTime().Date == DateTime.Today),
                    x => x.ReportType),
                AverageReportRenderTimeByReportType = string.Join(";", all.GroupBy(x => x.ReportType)
                    .ToDictionary(x => x.Key, x => x.Average(y => y.ResponseDateTimeUtc.Subtract(y.RequestDateTimeUtc).TotalMilliseconds))
                    .Select(x => $"{x.Key}={x.Value:F}").ToArray()),
                AverageReportRenderTimeByExportType = string.Join(";", all.GroupBy(x => x.ExportType)
                    .ToDictionary(x => x.Key, x => x.Average(y => y.ResponseDateTimeUtc.Subtract(y.RequestDateTimeUtc).TotalMilliseconds))
                    .Select(x => $"{x.Key}={x.Value:F}").ToArray()),
            };
        }

        private string CountGroupByAndJoin(IEnumerable<ReportRequest> all, Func<ReportRequest, string> selector)
        {
            return string.Join(
                ";",
                all.GroupBy(selector)
                    .ToDictionary(x => x.Key, x => x.Count())
                    .Select(x => $"{x.Key}={x.Value}").ToArray());
        }
    }
}