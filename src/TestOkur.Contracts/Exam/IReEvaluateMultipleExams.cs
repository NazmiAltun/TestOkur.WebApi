namespace TestOkur.Contracts.Exam
{
    using System.Collections.Generic;

    public interface IReEvaluateMultipleExams : IIntegrationEvent
    {
        IEnumerable<int> ExamIds { get; }
    }
}
