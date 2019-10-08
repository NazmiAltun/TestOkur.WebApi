namespace TestOkur.Report.Consumers
{
    using System.Linq;
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.Extensions.Logging;
    using TestOkur.Report.Domain;
    using TestOkur.Report.Events;
    using TestOkur.Report.Infrastructure.Repositories;

    public class EvaluateExamConsumer : IConsumer<IEvaluateExam>
    {
        private readonly IOpticalFormRepository _opticalFormRepository;
        private readonly ILogger<EvaluateExamConsumer> _logger;
        private readonly IEvaluator _evaluator;

        public EvaluateExamConsumer(IOpticalFormRepository opticalFormRepository, ILogger<EvaluateExamConsumer> logger, IEvaluator evaluator)
        {
            _opticalFormRepository = opticalFormRepository;
            _logger = logger;
            _evaluator = evaluator;
        }

        public async Task Consume(ConsumeContext<IEvaluateExam> context)
        {
            await ConsumeAsync(context.Message.ExamId);
        }

        public async Task ConsumeAsync(int examId)
        {
            _logger.LogInformation($"Evaluation for exam {examId} started...");
            var answerKeyForms = (await _opticalFormRepository
                    .GetAnswerKeyOpticalForms(examId))
                .ToList();
            _logger.LogInformation($"Answerkey forms count {answerKeyForms.Count}");
            var studentForms = (await _opticalFormRepository
                    .GetStudentOpticalFormsByExamIdAsync(examId))
                .ToList();
            _logger.LogInformation($"Student forms count {studentForms.Count}");
            studentForms = _evaluator.Evaluate(answerKeyForms, studentForms).ToList();

            await _opticalFormRepository.AddOrUpdateManyAsync(studentForms);
            _logger.LogInformation($"Evaluation for exam {examId} ended...");
        }
    }
}
