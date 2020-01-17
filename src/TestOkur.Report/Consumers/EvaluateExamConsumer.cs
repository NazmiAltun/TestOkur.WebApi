namespace TestOkur.Report.Consumers
{
    using MassTransit;
    using Microsoft.Extensions.Logging;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading.Tasks;
    using TestOkur.Report.Domain;
    using TestOkur.Report.Events;
    using TestOkur.Report.Infrastructure.Repositories;

    public class EvaluateExamConsumer : IConsumer<IEvaluateExam>
    {
        private static readonly ConcurrentDictionary<int, int> ExamIdsInProcess = new ConcurrentDictionary<int, int>();

        private readonly IStudentOpticalFormRepository _studentOpticalFormRepository;
        private readonly IAnswerKeyOpticalFormRepository _answerKeyOpticalFormRepository;
        private readonly ISchoolResultRepository _schoolResultRepository;
        private readonly ILogger<EvaluateExamConsumer> _logger;
        private readonly IEvaluator _evaluator;

        public EvaluateExamConsumer(
            IStudentOpticalFormRepository studentOpticalFormRepository,
            ILogger<EvaluateExamConsumer> logger,
            IEvaluator evaluator,
            IAnswerKeyOpticalFormRepository answerKeyOpticalFormRepository,
            ISchoolResultRepository schoolResultRepository)
        {
            _studentOpticalFormRepository = studentOpticalFormRepository;
            _logger = logger;
            _evaluator = evaluator;
            _schoolResultRepository = schoolResultRepository;
            _answerKeyOpticalFormRepository = answerKeyOpticalFormRepository;
        }

        public Task Consume(ConsumeContext<IEvaluateExam> context)
        {
            return ConsumeAsync(context.Message.ExamId);
        }

        public async Task ConsumeAsync(int examId)
        {
            while (!ExamIdsInProcess.TryAdd(examId, examId))
            {
                await Task.Delay(500);
            }

            try
            {
                await RunAsync(examId);
            }
            finally
            {
                ExamIdsInProcess.TryRemove(examId, out _);
            }
        }

        private async Task RunAsync(int examId)
        {
            var answerKeyForms = await _answerKeyOpticalFormRepository.GetByExamIdAsync(examId);
            var studentForms = await _studentOpticalFormRepository.GetStudentOpticalFormsByExamIdAsync(examId);
            studentForms = _evaluator.Evaluate(answerKeyForms, studentForms).ToList();

            await _studentOpticalFormRepository.AddOrUpdateManyAsync(studentForms);
            _logger.LogInformation($"Evaluation for exam {examId} ended...");

            if (answerKeyForms.First().SharedExam)
            {
                var results = _evaluator.EvaluateSchoolResults(studentForms);
                await _schoolResultRepository.AddOrUpdateManyAsync(results);
            }
        }
    }
}
