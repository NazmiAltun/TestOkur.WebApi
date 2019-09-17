namespace TestOkur.Report.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using TestOkur.Contracts.Lesson;
    using TestOkur.Report.Repositories;

    internal class LessonNameChangedConsumer : IConsumer<ILessonNameChanged>
    {
        private readonly IOpticalFormRepository _opticalFormRepository;

        public LessonNameChangedConsumer(IOpticalFormRepository opticalFormRepository)
        {
            _opticalFormRepository = opticalFormRepository;
        }

        public async Task Consume(ConsumeContext<ILessonNameChanged> context)
        {
            await _opticalFormRepository.UpdateLessonNameAsync(
                context.Message.LessonId,
                context.Message.NewLessonName);
        }
    }
}
