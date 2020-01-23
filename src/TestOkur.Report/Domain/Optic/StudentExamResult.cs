namespace TestOkur.Report.Domain.Optic
{
    using MongoDB.Bson;
    using System;

    public class StudentExamResult
    {
        public ObjectId Id { get; set; }

        public int ExamId { get; set; }

        public int StudentId { get; set; }

        public DateTime CreatedOnDateTimeUtc { get; set; }

        public SectionResult[] SectionResults { get; set; }
    }
}