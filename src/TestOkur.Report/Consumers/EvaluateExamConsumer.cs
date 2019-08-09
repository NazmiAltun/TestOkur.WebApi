namespace TestOkur.Report.Consumers
{
	using System.Linq;
	using System.Threading.Tasks;
	using MassTransit;
	using Microsoft.Extensions.Logging;
	using TestOkur.Optic;
	using TestOkur.Report.Events;
	using TestOkur.Report.Repositories;

	public class EvaluateExamConsumer : IConsumer<IEvaluateExam>
	{
		private readonly IOpticalFormRepository _opticalFormRepository;
		private readonly ILogger<EvaluateExamConsumer> _logger;

		public EvaluateExamConsumer(IOpticalFormRepository opticalFormRepository, ILogger<EvaluateExamConsumer> logger)
		{
			_opticalFormRepository = opticalFormRepository;
			_logger = logger;
		}

		public async Task Consume(ConsumeContext<IEvaluateExam> context)
		{
			await ConsumeAsync(context.Message.ExamId);
		}

		public async Task ConsumeAsync(int examId)
		{
			_logger.LogWarning($"Evaluation for exam {examId} started...");
			var answerKeyForms = (await _opticalFormRepository
					.GetAnswerKeyOpticalForms(examId))
				.ToList();
			_logger.LogWarning($"Answerkey forms count {answerKeyForms.Count}");
			var studentForms = (await _opticalFormRepository
					.GetStudentOpticalFormsByExamIdAsync(examId))
				.ToList();
			_logger.LogWarning($"Student forms count {studentForms.Count}");
			var evaluator = new Evaluator(answerKeyForms);
			studentForms = evaluator.Evaluate(studentForms);

			await _opticalFormRepository.AddOrUpdateManyAsync(studentForms);
			_logger.LogWarning($"Evaluation for exam {examId} ended...");
		}
	}
}
