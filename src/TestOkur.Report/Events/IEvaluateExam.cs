namespace TestOkur.Report.Events
{
    using TestOkur.Contracts;

    public interface IEvaluateExam : IIntegrationEvent
    {
        int ExamId { get; }
    }
}
