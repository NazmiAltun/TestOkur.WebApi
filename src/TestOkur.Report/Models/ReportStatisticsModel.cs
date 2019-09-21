namespace TestOkur.Report.Models
{
    using System.Collections.Generic;

    public class ReportStatisticsModel
    {
        public int TotalCount { get; set; }

        public int TodayCount { get; set; }

        public IEnumerable<KeyValuePair<string, int>> CountByReportTypeTotal { get; set; }

        public IEnumerable<KeyValuePair<string, int>> CountByReportTypeToday { get; set; }

        public IEnumerable<KeyValuePair<string, int>> CountByExportTypeTotal { get; set; }

        public IEnumerable<KeyValuePair<string, int>> CountByExportTypeToday { get; set; }

        public IEnumerable<KeyValuePair<string, int>> AverageReportRenderTimeByReportType { get; set; }

        public IEnumerable<KeyValuePair<string, int>> AverageReportRenderTimeByExportType { get; set; }
    }
}
