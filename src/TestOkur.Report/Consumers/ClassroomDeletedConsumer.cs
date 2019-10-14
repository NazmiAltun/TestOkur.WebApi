namespace TestOkur.Report.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using TestOkur.Contracts.Classroom;
    using TestOkur.Report.Infrastructure.Repositories;

    internal class ClassroomDeletedConsumer : IConsumer<IClassroomDeleted>
    {
        private readonly IStudentOpticalFormRepository _studentOpticalFormRepository;
        private readonly EvaluateExamConsumer _evaluateExamConsumer;

        public ClassroomDeletedConsumer(
            IStudentOpticalFormRepository studentOpticalFormRepository,
            EvaluateExamConsumer evaluateExamConsumer)
        {
            _studentOpticalFormRepository = studentOpticalFormRepository;
            _evaluateExamConsumer = evaluateExamConsumer;
        }

        public async Task Consume(ConsumeContext<IClassroomDeleted> context)
        {
            await _studentOpticalFormRepository
                .DeleteByClassroomIdAsync(context.Message.ClassroomId);
            var examIds = await _studentOpticalFormRepository.GetExamIdsAsync(
                f => f.ClassroomId, context.Message.ClassroomId);

            if (_evaluateExamConsumer != null)
            {
                foreach (var examId in examIds)
                {
                    await _evaluateExamConsumer.ConsumeAsync(examId);
                }
            }
        }
    }
}
