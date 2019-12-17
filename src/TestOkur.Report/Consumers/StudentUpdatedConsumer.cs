namespace TestOkur.Report.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using TestOkur.Contracts.Student;
    using TestOkur.Report.Infrastructure.Repositories;

    internal class StudentUpdatedConsumer : IConsumer<IStudentUpdated>
    {
        private readonly IStudentOpticalFormRepository _studentOpticalFormRepository;

        public StudentUpdatedConsumer(IStudentOpticalFormRepository studentOpticalFormRepository)
        {
            _studentOpticalFormRepository = studentOpticalFormRepository;
        }

        public Task Consume(ConsumeContext<IStudentUpdated> context)
        {
            return _studentOpticalFormRepository.UpdateStudentAsync(context.Message);
        }
    }
}
