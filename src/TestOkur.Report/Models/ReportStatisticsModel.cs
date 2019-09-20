namespace TestOkur.Report.Models
{
    public class ReportStatisticsModel
    {
        public int TotalCount { get; set; }

        public int TodayCount { get; set; }

        public string CountByReportTypeTotal { get; set; }

        public string CountByReportTypeToday { get; set; }

        public string CountByExportTypeTotal { get; set; }

        public string CountByExportTypeToday { get; set; }

        public string AverageReportRenderTimeByReportType { get; set; }

        public string AverageReportRenderTimeByExportType { get; set; }
    }
}
