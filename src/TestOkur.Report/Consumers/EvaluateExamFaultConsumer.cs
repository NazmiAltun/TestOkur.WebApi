namespace TestOkur.Report.Consumers
{
	using System.Threading.Tasks;
	using MassTransit;
	using Microsoft.Extensions.Logging;
	using Newtonsoft.Json;
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
			_logger.LogError(JsonConvert.SerializeObject(context.Message));

			return Task.CompletedTask;
		}
	}
}
