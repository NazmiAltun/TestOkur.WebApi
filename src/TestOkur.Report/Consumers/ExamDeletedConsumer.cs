namespace TestOkur.Report.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using TestOkur.Contracts.Exam;
    using TestOkur.Report.Infrastructure.Repositories;

    internal class ExamDeletedConsumer : IConsumer<IExamDeleted>
    {
        private readonly IStudentOpticalFormRepository _studentOpticalFormRepository;
        private readonly IAnswerKeyOpticalFormRepository _answerKeyOpticalFormRepository;

        public ExamDeletedConsumer(IStudentOpticalFormRepository studentOpticalFormRepository, IAnswerKeyOpticalFormRepository answerKeyOpticalFormRepository)
        {
            _studentOpticalFormRepository = studentOpticalFormRepository;
            _answerKeyOpticalFormRepository = answerKeyOpticalFormRepository;
        }

        public async Task Consume(ConsumeContext<IExamDeleted> context)
        {
            await _answerKeyOpticalFormRepository.DeleteByExamIdAsync(context.Message.ExamId);
            await _studentOpticalFormRepository.DeleteByExamIdAsync(context.Message.ExamId);
        }
    }
}
