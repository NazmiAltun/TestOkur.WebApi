namespace TestOkur.Report.Infrastructure.Repositories
{
    using MongoDB.Bson.Serialization;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TestOkur.Report.Configuration;
    using TestOkur.Report.Models;

    public class ReportRequestRepository : IReportRequestRepository
    {
        private readonly TestOkurContext _context;

        public ReportRequestRepository(ReportConfiguration configuration)
        {
            _context = new TestOkurContext(configuration);
        }

        public Task AddAsync(ReportRequest reportRequest)
        {
            return _context.ReportRequests.InsertOneAsync(reportRequest);
        }

        public async Task<ReportStatisticsModel> GetStatisticsAsync()
        {
            var all = (await _context.ReportRequests
                    .Find(Builders<ReportRequest>.Filter.Empty)
                    .Project(Builders<ReportRequest>.Projection
                        .Include("ExportType")
                        .Include("RequestDateTimeUtc")
                        .Include("ResponseDateTimeUtc")
                        .Include("ReportType"))
                    .ToListAsync())
                .Select(bson => BsonSerializer.Deserialize<ReportRequest>(bson))
                .ToList();

            var model = new ReportStatisticsModel()
            {
                TotalCount = all.Count,
            };
            var countByReportTypeTotal = new Dictionary<string, int>(20);
            var countByExportTypeTotal = new Dictionary<string, int>(10);
            var countByReportTypeToday = new Dictionary<string, int>(20);
            var countByExportTypeToday = new Dictionary<string, int>(10);
            var reportRenderTimeByReportType = new Dictionary<string, List<int>>(20);
            var reportRenderTimeByExportType = new Dictionary<string, List<int>>(10);

            foreach (var request in all)
            {
                var today = request.RequestDateTimeUtc.ToLocalTime().Date == DateTime.Today;

                if (today)
                {
                    model.TodayCount++;
                    if (!countByReportTypeToday.TryAdd(request.ReportType, 1))
                    {
                        countByReportTypeToday[request.ReportType]++;
                    }

                    if (!countByExportTypeToday.TryAdd(request.ExportType, 1))
                    {
                        countByExportTypeToday[request.ExportType]++;
                    }
                }

                if (!countByReportTypeTotal.TryAdd(request.ReportType, 1))
                {
                    countByReportTypeTotal[request.ReportType]++;
                }

                if (!countByExportTypeTotal.TryAdd(request.ExportType, 1))
                {
                    countByExportTypeTotal[request.ExportType]++;
                }

                var renderTime = (int)request.ResponseDateTimeUtc.Subtract(request.RequestDateTimeUtc).TotalMilliseconds;
                reportRenderTimeByReportType.TryAdd(request.ReportType, new List<int>());
                reportRenderTimeByReportType[request.ReportType].Add(renderTime);

                reportRenderTimeByExportType.TryAdd(request.ExportType, new List<int>());
                reportRenderTimeByExportType[request.ExportType].Add(renderTime);
            }

            model.CountByExportTypeTotal = countByExportTypeTotal.OrderByDescending(x => x.Value);
            model.CountByReportTypeTotal = countByReportTypeTotal.OrderByDescending(x => x.Value);
            model.CountByExportTypeToday = countByExportTypeToday.OrderByDescending(x => x.Value);
            model.CountByReportTypeToday = countByReportTypeToday.OrderByDescending(x => x.Value);
            model.AverageReportRenderTimeByExportType =
                reportRenderTimeByExportType.Select(pair =>
                        new KeyValuePair<string, int>(pair.Key, (int)pair.Value.Average()))
                    .OrderByDescending(x => x.Value);
            model.AverageReportRenderTimeByReportType =
                reportRenderTimeByReportType.Select(pair =>
                    new KeyValuePair<string, int>(pair.Key, (int)pair.Value.Average()))
                    .OrderByDescending(x => x.Value);

            return model;
        }
    }
}