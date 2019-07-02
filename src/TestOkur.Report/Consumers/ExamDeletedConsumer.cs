namespace TestOkur.Report.Consumers
{
	using System.Threading.Tasks;
	using MassTransit;
	using TestOkur.Contracts.Exam;
	using TestOkur.Report.Repositories;

	internal class ExamDeletedConsumer : IConsumer<IExamDeleted>
	{
		private readonly IOpticalFormRepository _opticalFormRepository;

		public ExamDeletedConsumer(IOpticalFormRepository opticalFormRepository)
		{
			_opticalFormRepository = opticalFormRepository;
		}

		public async Task Consume(ConsumeContext<IExamDeleted> context)
		{
			await _opticalFormRepository.DeleteByExamIdAsync(context.Message.ExamId);
		}
	}
}
