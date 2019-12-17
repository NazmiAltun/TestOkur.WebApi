namespace TestOkur.Report.Consumers
{
    using MassTransit;
    using System.Threading.Tasks;
    using TestOkur.Contracts.Exam;
    using TestOkur.Report.Infrastructure.Repositories;

    internal class ExamCreatedConsumer : IConsumer<IExamCreated>
    {
        private readonly IAnswerKeyOpticalFormRepository _answerKeyOpticalFormRepository;

        public ExamCreatedConsumer(IAnswerKeyOpticalFormRepository answerKeyOpticalFormRepository)
        {
            _answerKeyOpticalFormRepository = answerKeyOpticalFormRepository;
        }

        public Task Consume(ConsumeContext<IExamCreated> context)
        {
            var forms = context.Message.AnswerKeyOpticalForms;

            foreach (var form in forms)
            {
                form.ExamId = context.Message.ExamId;
                form.IncorrectEliminationRate = context.Message.IncorrectEliminationRate;
                form.ExamDate = context.Message.ExamDate;
                form.ExamName = context.Message.ExamName;
                form.SharedExam = context.Message.Shared;
                form.ExamTypeName = context.Message.ExamTypeName;
            }

            return _answerKeyOpticalFormRepository.AddManyAsync(forms);
        }
    }
}
