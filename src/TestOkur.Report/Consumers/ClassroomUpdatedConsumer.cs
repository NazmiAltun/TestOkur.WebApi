namespace TestOkur.Report.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using TestOkur.Contracts.Classroom;
    using TestOkur.Report.Repositories;

    internal class ClassroomUpdatedConsumer : IConsumer<IClassroomUpdated>
	{
		private readonly IOpticalFormRepository _opticalFormRepository;

		public ClassroomUpdatedConsumer(IOpticalFormRepository opticalFormRepository)
		{
			_opticalFormRepository = opticalFormRepository;
		}

		public async Task Consume(ConsumeContext<IClassroomUpdated> context)
		{
			await _opticalFormRepository
				.UpdateClassroomAsync(
					context.Message.ClassroomId,
					context.Message.Grade,
					context.Message.Name);
		}
	}
}
