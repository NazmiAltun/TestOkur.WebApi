namespace TestOkur.Report.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using TestOkur.Contracts.Lesson;
    using TestOkur.Report.Infrastructure.Repositories;

    internal class LessonNameChangedConsumer : IConsumer<ILessonNameChanged>
    {
        private readonly IStudentOpticalFormRepository _studentOpticalFormRepository;
        private readonly IAnswerKeyOpticalFormRepository _answerKeyOpticalFormRepository;

        public LessonNameChangedConsumer(IStudentOpticalFormRepository studentOpticalFormRepository, IAnswerKeyOpticalFormRepository answerKeyOpticalFormRepository)
        {
            _studentOpticalFormRepository = studentOpticalFormRepository;
            _answerKeyOpticalFormRepository = answerKeyOpticalFormRepository;
        }

        public async Task Consume(ConsumeContext<ILessonNameChanged> context)
        {
            await _answerKeyOpticalFormRepository.UpdateLessonNameAsync(
                context.Message.LessonId,
                context.Message.NewLessonName);
            await _studentOpticalFormRepository.UpdateLessonNameAsync(
                context.Message.LessonId,
                context.Message.NewLessonName);
        }
    }
}
