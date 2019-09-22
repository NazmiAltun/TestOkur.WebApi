namespace TestOkur.Report.Consumers
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.Extensions.Logging;
    using TestOkur.Contracts.Exam;

    public class ReEvaluateAllTheExamsConsumer : IConsumer<IReEvaluateMultipleExams>
    {
        private readonly EvaluateExamConsumer _evaluateExamConsumer;
        private readonly ILogger<ReEvaluateAllTheExamsConsumer> _logger;

        public ReEvaluateAllTheExamsConsumer(
            EvaluateExamConsumer evaluateExamConsumer,
            ILogger<ReEvaluateAllTheExamsConsumer> logger)
        {
            _evaluateExamConsumer = evaluateExamConsumer;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IReEvaluateMultipleExams> context)
        {
            var stopwatch = Stopwatch.StartNew();
            foreach (var examId in context.Message.ExamIds)
            {
                try
                {
                    await _evaluateExamConsumer.ConsumeAsync(examId);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception while evaluating exam with ID:{examId} : {ex}");
                }
            }

            stopwatch.Stop();
            _logger.LogWarning(
                $"{context.Message.ExamIds.Count()} amount of exams re-evaluated in {stopwatch.Elapsed.TotalSeconds:F1} seconds");
        }
    }
}
