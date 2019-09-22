namespace TestOkur.WebApi.Application.Exam.Commands
{
    using System.Collections.Generic;
    using TestOkur.Contracts;
    using TestOkur.Contracts.Exam;

    public class ReEvaluateMultipleExams : IntegrationEvent, IReEvaluateMultipleExams
    {
        public ReEvaluateMultipleExams(IEnumerable<int> examIds)
        {
            ExamIds = examIds;
        }

        public IEnumerable<int> ExamIds { get; }
    }
}
