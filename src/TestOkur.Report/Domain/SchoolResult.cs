namespace TestOkur.Report.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TestOkur.Optic.Form;

    public class SchoolResult
    {
        public SchoolResult(StudentOpticalForm form)
        {
            ExamId = form.ExamId;
            SchoolId = form.SchoolId;
            SchoolName = form.SchoolName;
            CityId = form.CityId;
            CityName = form.CityName;
            DistrictId = form.DistrictId;
            DistrictName = form.DistrictName;
            ScoreAverage = form.SchoolScoreAverage;
            StudentCount = form.SchoolAttendanceCount;
            LessonNetAverages = form.Sections
                .ToDictionary(s => s.LessonName, s => s.SchoolAverageNet);
        }

        public SchoolResult()
        {
        }

        public Guid Id { get; set; } = Guid.NewGuid();

        public int ExamId { get; set; }

        public int SchoolId { get; set; }

        public string SchoolName { get; set; }

        public int CityId { get; set; }

        public string CityName { get; set; }

        public int DistrictId { get; set; }

        public string DistrictName { get; set; }

        public int ClassroomCount { get; set; }

        public int StudentCount { get; set; }

        public float ScoreAverage { get; set; }

        public float SuccessPercent { get; set; }

        public int DistrictOrder { get; set; }

        public int CityOrder { get; set; }

        public int GeneralOrder { get; set; }

        public Dictionary<string, float> LessonNetAverages { get; set; }
    }
}