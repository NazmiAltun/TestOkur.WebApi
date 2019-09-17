namespace TestOkur.Report.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.Extensions.Logging;
    using TestOkur.Report.Events;

    internal class EvaluateExamFaultConsumer : IConsumer<Fault<IEvaluateExam>>
	{
		private readonly ILogger<EvaluateExamFaultConsumer> _logger;

		public EvaluateExamFaultConsumer(ILogger<EvaluateExamFaultConsumer> logger)
		{
			_logger = logger;
		}

		public Task Consume(ConsumeContext<Fault<IEvaluateExam>> context)
		{
			foreach (var exception in context.Message.Exceptions)
			{
				_logger.LogError($"Exam : {context.Message.Message.ExamId} : " +
				                 $"{exception.ExceptionType} : {exception.Source} ==>" +
				                 $" {exception.Message} : {exception.StackTrace}");
			}

			return Task.CompletedTask;
		}
	}
}
