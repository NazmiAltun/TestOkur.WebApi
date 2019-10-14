namespace TestOkur.Report.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using TestOkur.Contracts.Student;
    using TestOkur.Report.Infrastructure.Repositories;

    internal class StudentDeletedConsumer : IConsumer<IStudentDeleted>
    {
        private readonly EvaluateExamConsumer _evaluateExamConsumer;
        private readonly IStudentOpticalFormRepository _studentOpticalFormRepository;

        public StudentDeletedConsumer(
            IStudentOpticalFormRepository studentOpticalFormRepository,
            EvaluateExamConsumer evaluateExamConsumer)
        {
            _studentOpticalFormRepository = studentOpticalFormRepository;
            _evaluateExamConsumer = evaluateExamConsumer;
        }

        public async Task Consume(ConsumeContext<IStudentDeleted> context)
        {
            await _studentOpticalFormRepository
                .DeleteByStudentIdAsync(context.Message.StudentId);

            var examIds = await _studentOpticalFormRepository.GetExamIdsAsync(
                f => f.StudentId, context.Message.StudentId);

            foreach (var examId in examIds)
            {
                await _evaluateExamConsumer?.ConsumeAsync(examId);
            }
        }
    }
}
