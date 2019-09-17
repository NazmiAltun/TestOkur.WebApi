namespace TestOkur.Report.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using TestOkur.Contracts.Classroom;
    using TestOkur.Report.Repositories;

    internal class ClassroomDeletedConsumer : IConsumer<IClassroomDeleted>
    {
        private readonly IOpticalFormRepository _opticalFormRepository;
        private readonly EvaluateExamConsumer _evaluateExamConsumer;

        public ClassroomDeletedConsumer(
            IOpticalFormRepository opticalFormRepository,
            EvaluateExamConsumer evaluateExamConsumer)
        {
            _opticalFormRepository = opticalFormRepository;
            _evaluateExamConsumer = evaluateExamConsumer;
        }

        public async Task Consume(ConsumeContext<IClassroomDeleted> context)
        {
            await _opticalFormRepository
                .DeleteByClassroomIdAsync(context.Message.ClassroomId);
            var examIds = await _opticalFormRepository.GetExamIdsAsync(
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
