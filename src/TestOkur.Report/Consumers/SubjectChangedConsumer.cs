namespace TestOkur.Report.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using TestOkur.Contracts.Lesson;
    using TestOkur.Report.Infrastructure.Repositories;

    internal class SubjectChangedConsumer : IConsumer<ISubjectChanged>
    {
        private readonly IOpticalFormRepository _opticalFormRepository;

        public SubjectChangedConsumer(IOpticalFormRepository opticalFormRepository)
        {
            _opticalFormRepository = opticalFormRepository;
        }

        public async Task Consume(ConsumeContext<ISubjectChanged> context)
        {
            await _opticalFormRepository.UpdateSubjectNameAsync(
                context.Message.SubjectId,
                context.Message.NewName);
        }
    }
}
