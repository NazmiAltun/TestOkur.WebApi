namespace TestOkur.Report.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using TestOkur.Contracts.Student;
    using TestOkur.Report.Infrastructure.Repositories;

    internal class StudentUpdatedConsumer : IConsumer<IStudentUpdated>
    {
        private readonly IOpticalFormRepository _opticalFormRepository;

        public StudentUpdatedConsumer(IOpticalFormRepository opticalFormRepository)
        {
            _opticalFormRepository = opticalFormRepository;
        }

        public async Task Consume(ConsumeContext<IStudentUpdated> context)
        {
            await _opticalFormRepository.UpdateStudentAsync(context.Message);
        }
    }
}
