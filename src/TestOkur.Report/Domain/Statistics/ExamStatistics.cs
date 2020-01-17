namespace TestOkur.Report.Domain.Statistics
{
    using System.Collections.Generic;

    public class ExamStatistics
    {
        public static ExamStatistics Empty = new ExamStatistics();

        public int ExamId { get; set; }

        // Attendance
        public int GeneralAttendanceCount { get; set; }

        public Dictionary<int, int> CityAttendanceCounts { get; set; }

        public Dictionary<int, int> DistrictAttendanceCounts { get; set; }

        public Dictionary<int, int> ClassroomAttendanceCounts { get; set; }

        public Dictionary<int, int> SchoolAttendanceCounts { get; set; }

        // Average
        public float AverageScore { get; set; }

        public Dictionary<int, float> CityAverageScores { get; set; }

        public Dictionary<int, float> DistrictAverageScores { get; set; }

        public Dictionary<int, float> SchoolAverageScores { get; set; }

        public Dictionary<int, float> ClassroomAverageScores { get; set; }

        public Dictionary<string, SectionAverage> SectionAverages { get; set; }
    }
}
