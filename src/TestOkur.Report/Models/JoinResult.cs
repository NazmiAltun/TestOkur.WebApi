namespace TestOkur.Report.Models
{
    using System.Collections.Generic;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Domain.Statistics;

    public class JoinResult
    {
        public ExamStatistics ExamStatistics { get; set; }

        public List<StudentOpticalForm> Forms { get; set; }
    }
}
