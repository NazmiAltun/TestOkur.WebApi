namespace TestOkur.Report.Consumers
{
	using System.Threading.Tasks;
	using MassTransit;
	using TestOkur.Contracts.Exam;
	using TestOkur.Report.Repositories;

	internal class ExamCreatedConsumer : IConsumer<IExamCreated>
	{
		private readonly IOpticalFormRepository _opticalFormRepository;

		public ExamCreatedConsumer(IOpticalFormRepository opticalFormRepository)
		{
			_opticalFormRepository = opticalFormRepository;
		}

		public async Task Consume(ConsumeContext<IExamCreated> context)
		{
			var forms = context.Message.AnswerKeyOpticalForms;

			foreach (var form in forms)
			{
				form.ExamId = context.Message.ExamId;
				form.IncorrectEliminationRate = context.Message.IncorrectEliminationRate;
				form.ExamDate = context.Message.ExamDate;
				form.ExamName = context.Message.ExamName;
				form.ExamTypeName = context.Message.ExamTypeName;
			}

			await _opticalFormRepository.AddManyAsync(forms);
		}
	}
}
