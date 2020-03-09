namespace TestOkur.Notification.Models
{
    using System.Collections.Generic;

    public class StatisticsReadModel
    {
        public int TotalStudentCount => TotalESchoolStudentCount + TotalBulkStudentCount + TotalSingleEntryStudentCount;

        public int TodayStudentCount => TodayESchoolStudentCount + TodayBulkStudentCount + TodaySingleEntryStudentCount;

        public int TodayScannedStudentFormCount => TodayScannedStudentFormCountByCamera + TodayScannedStudentFormCountByFile;

        public int TotalScannedStudentFormCount => TotalScannedStudentFormCountByCamera + TotalScannedStudentFormCountByFile;

        public int TotalESchoolStudentCount { get; set; }

        public int TotalBulkStudentCount { get; set; }

        public int TotalSingleEntryStudentCount { get; set; }

        public int TotalScannedStudentFormCountByCamera { get; set; }

        public int TotalScannedStudentFormCountByFile { get; set; }

        public int TotalExamCount { get; set; }

        public int TodayESchoolStudentCount { get; set; }

        public int TodayBulkStudentCount { get; set; }

        public int TodaySingleEntryStudentCount { get; set; }

        public int TodayScannedStudentFormCountByCamera { get; set; }

        public int TodayScannedStudentFormCountByFile { get; set; }

        public int TodayExamCount { get; set; }

        public IEnumerable<ExamReadModel> SharedExams { get; set; }
    }
}
