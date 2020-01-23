namespace TestOkur.Report.Domain.Optic
{
    using MongoDB.Bson;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class StudentExamResult
    {
        public StudentExamResult(
            int examId,
            int studentId,
            int classroomId,
            int schoolId,
            int districtId,
            int cityId,
            SectionResult[] sectionResults)
        {
            ExamId = examId;
            StudentId = studentId;
            ClassroomId = classroomId;
            SchoolId = schoolId;
            DistrictId = districtId;
            CityId = cityId;
            SectionResults = sectionResults;
            CreatedOnDateTimeUtc = DateTime.UtcNow;
        }

        private StudentExamResult()
        {
        }

        public ObjectId Id { get; set; }

        public int ExamId { get; set; }

        public int StudentId { get; set; }

        public int ClassroomId { get; set; }

        public int SchoolId { get; set; }

        public int DistrictId { get; set; }

        public int CityId { get; set; }

        public DateTime CreatedOnDateTimeUtc { get; set; }

        public SectionResult[] SectionResults { get; set; }

        public Dictionary<string, float> Scores { get; set; }

        public float Net => SectionResults.Sum(s => s.Net);

        public int QuestionCount => SectionResults.Sum(s => s.QuestionCount);
    }
}