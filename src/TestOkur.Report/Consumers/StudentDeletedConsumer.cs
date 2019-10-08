namespace TestOkur.Report.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using TestOkur.Contracts.Student;
    using TestOkur.Report.Infrastructure.Repositories;

    internal class StudentDeletedConsumer : IConsumer<IStudentDeleted>
    {
        private readonly EvaluateExamConsumer _evaluateExamConsumer;
        private readonly IOpticalFormRepository _opticalFormRepository;

        public StudentDeletedConsumer(
            IOpticalFormRepository opticalFormRepository,
            EvaluateExamConsumer evaluateExamConsumer)
        {
            _opticalFormRepository = opticalFormRepository;
            _evaluateExamConsumer = evaluateExamConsumer;
        }

        public async Task Consume(ConsumeContext<IStudentDeleted> context)
        {
            await _opticalFormRepository
                .DeleteByStudentIdAsync(context.Message.StudentId);

            var examIds = await _opticalFormRepository.GetExamIdsAsync(
                f => f.StudentId, context.Message.StudentId);

            foreach (var examId in examIds)
            {
                await _evaluateExamConsumer?.ConsumeAsync(examId);
            }
        }
    }
}
