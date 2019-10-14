namespace TestOkur.Report.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using TestOkur.Contracts.Lesson;
    using TestOkur.Report.Infrastructure.Repositories;

    internal class SubjectChangedConsumer : IConsumer<ISubjectChanged>
    {
        private readonly IStudentOpticalFormRepository _studentOpticalFormRepository;
        private readonly IAnswerKeyOpticalFormRepository _answerKeyOpticalFormRepository;

        public SubjectChangedConsumer(IStudentOpticalFormRepository studentOpticalFormRepository, IAnswerKeyOpticalFormRepository answerKeyOpticalFormRepository)
        {
            _studentOpticalFormRepository = studentOpticalFormRepository;
            _answerKeyOpticalFormRepository = answerKeyOpticalFormRepository;
        }

        public async Task Consume(ConsumeContext<ISubjectChanged> context)
        {
            await _answerKeyOpticalFormRepository.UpdateSubjectNameAsync(
                context.Message.SubjectId,
                context.Message.NewName);
            await _studentOpticalFormRepository.UpdateSubjectNameAsync(
                context.Message.SubjectId,
                context.Message.NewName);
        }
    }
}
