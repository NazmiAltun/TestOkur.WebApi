namespace TestOkur.Report.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using TestOkur.Contracts.Classroom;
    using TestOkur.Report.Infrastructure.Repositories;

    internal class ClassroomUpdatedConsumer : IConsumer<IClassroomUpdated>
    {
        private readonly IStudentOpticalFormRepository _studentOpticalFormRepository;

        public ClassroomUpdatedConsumer(IStudentOpticalFormRepository studentOpticalFormRepository)
        {
            _studentOpticalFormRepository = studentOpticalFormRepository;
        }

        public async Task Consume(ConsumeContext<IClassroomUpdated> context)
        {
            await _studentOpticalFormRepository
                .UpdateClassroomAsync(
                    context.Message.ClassroomId,
                    context.Message.Grade,
                    context.Message.Name);
        }
    }
}
