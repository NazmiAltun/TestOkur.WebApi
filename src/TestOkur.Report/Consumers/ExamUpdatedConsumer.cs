namespace TestOkur.Report.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using TestOkur.Contracts.Exam;
    using TestOkur.Report.Infrastructure.Repositories;

    internal class ExamUpdatedConsumer : IConsumer<IExamUpdated>
    {
        private readonly IAnswerKeyOpticalFormRepository _answerKeyOpticalFormRepository;
        private readonly EvaluateExamConsumer _evaluateExamConsumer;

        public ExamUpdatedConsumer(IAnswerKeyOpticalFormRepository answerKeyOpticalFormRepository, EvaluateExamConsumer evaluateExamConsumer)
        {
            _evaluateExamConsumer = evaluateExamConsumer;
            _answerKeyOpticalFormRepository = answerKeyOpticalFormRepository;
        }

        public async Task Consume(ConsumeContext<IExamUpdated> context)
        {
            var forms = context.Message.AnswerKeyOpticalForms;

            foreach (var form in forms)
            {
                form.ExamId = context.Message.ExamId;
                form.ExamDate = context.Message.ExamDate;
                form.ExamName = context.Message.ExamName;
                form.SharedExam = context.Message.Shared;
                form.IncorrectEliminationRate = context.Message.IncorrectEliminationRate;
            }

            await _answerKeyOpticalFormRepository.DeleteByExamIdAsync(context.Message.ExamId);
            await _answerKeyOpticalFormRepository.AddManyAsync(forms);
            if (_evaluateExamConsumer != null)
            {
                await _evaluateExamConsumer.ConsumeAsync(context.Message.ExamId);
            }
        }
    }
}
