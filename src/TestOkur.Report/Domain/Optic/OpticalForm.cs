namespace TestOkur.Report.Domain.Optic
{
    using MongoDB.Bson;
    using System;

    public abstract class OpticalForm
    {
        public ObjectId Id { get; set; }

        public int ExamId { get; set; }

        public int SchoolId { get; set; }

        public DateTime CreatedOnDateTimeUtc { get; set; }
    }
}