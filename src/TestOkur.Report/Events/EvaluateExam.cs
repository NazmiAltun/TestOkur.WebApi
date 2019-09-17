namespace TestOkur.Report.Events
{
    using TestOkur.Contracts;

    public class EvaluateExam : IntegrationEvent, IEvaluateExam
	{
		public EvaluateExam(int examId)
		{
			ExamId = examId;
		}

		public int ExamId { get; }
	}
}
