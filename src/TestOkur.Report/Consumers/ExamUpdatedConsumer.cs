namespace TestOkur.Report.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using TestOkur.Contracts.Exam;
    using TestOkur.Report.Repositories;

    internal class ExamUpdatedConsumer : IConsumer<IExamUpdated>
	{
		private readonly IOpticalFormRepository _opticalFormRepository;
		private readonly EvaluateExamConsumer _evaluateExamConsumer;

		public ExamUpdatedConsumer(IOpticalFormRepository opticalFormRepository, EvaluateExamConsumer evaluateExamConsumer)
		{
			_opticalFormRepository = opticalFormRepository;
			_evaluateExamConsumer = evaluateExamConsumer;
		}

		public async Task Consume(ConsumeContext<IExamUpdated> context)
		{
			var forms = context.Message.AnswerKeyOpticalForms;

			foreach (var form in forms)
			{
				form.ExamId = context.Message.ExamId;
				form.ExamDate = context.Message.ExamDate;
				form.ExamName = context.Message.ExamName;
				form.IncorrectEliminationRate = context.Message.IncorrectEliminationRate;
			}

			await _opticalFormRepository.DeleteAnswerKeyOpticalFormsByExamIdAsync(context.Message.ExamId);
			await _opticalFormRepository.AddManyAsync(forms);
			if (_evaluateExamConsumer != null)
			{
				await _evaluateExamConsumer.ConsumeAsync(context.Message.ExamId);
			}
		}
	}
}
