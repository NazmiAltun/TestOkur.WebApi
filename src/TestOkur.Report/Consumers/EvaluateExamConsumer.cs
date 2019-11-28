namespace TestOkur.Report.Consumers
{
    using System.Collections.Concurrent;
    using MassTransit;
    using Microsoft.Extensions.Logging;
    using System.Linq;
    using System.Threading;
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

        public async Task Consume(ConsumeContext<IEvaluateExam> context)
        {
            await ConsumeAsync(context.Message.ExamId);
        }

        public async Task ConsumeAsync(int examId)
        {
            while (!ExamIdsInProcess.TryAdd(examId, examId))
            {
                await Task.Delay(100);
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
            _logger.LogInformation($"Evaluation for exam {examId} started...");
            var answerKeyForms = (await _answerKeyOpticalFormRepository.GetByExamIdAsync(examId))
                .ToList();
            _logger.LogInformation($"Answerkey forms count {answerKeyForms.Count}");
            var studentForms = (await _studentOpticalFormRepository
                    .GetStudentOpticalFormsByExamIdAsync(examId))
                .ToList();
            _logger.LogInformation($"Student forms count {studentForms.Count}");
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
