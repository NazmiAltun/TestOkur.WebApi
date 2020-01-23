namespace TestOkur.Report.Domain.Optic
{
    using System.Collections.Generic;
    using TestOkur.Report.Domain.Statistics;

    public class EvaluationResult
    {
        public EvaluationResult()
        {
            StudentExamResults = new List<StudentExamResult>();
            StudentSectionAnswerResults = new List<StudentSectionAnswerResult>();
        }

        public List<StudentExamResult> StudentExamResults { get; set; }

        public List<StudentSectionAnswerResult> StudentSectionAnswerResults { get; set; }

        public ExamStatistics ExamStatistics { get; set; }
    }
}