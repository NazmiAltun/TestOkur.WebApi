using System.Collections.Generic;

namespace TestOkur.Report.Domain.Optic
{
    public class EvaluationResult
    {
        public List<StudentExamResult> StudentExamResults { get; set; }

        public List<StudentSectionAnswerResult> StudentSectionAnswerResults { get; set; }
    }
}