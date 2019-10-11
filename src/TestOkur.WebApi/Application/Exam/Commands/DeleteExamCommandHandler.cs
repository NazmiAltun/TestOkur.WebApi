namespace TestOkur.WebApi.Application.Exam.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using TestOkur.Data;
    using TestOkur.Infrastructure.CommandsQueries;

    public sealed class DeleteExamCommandHandler : RequestHandlerAsync<DeleteExamCommand>
    {
        private readonly IApplicationDbContextFactory _dbContextFactory;
        private readonly IPublishEndpoint _publishEndpoint;

        public DeleteExamCommandHandler(IPublishEndpoint publishEndpoint, IApplicationDbContextFactory dbContextFactory)
        {
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            _dbContextFactory = dbContextFactory;
        }

        [ClearCache(2)]
        public override async Task<DeleteExamCommand> HandleAsync(
            DeleteExamCommand command,
            CancellationToken cancellationToken = default)
        {
            using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                var exam = await GetAsync(dbContext, command, cancellationToken);

                if (exam != null)
                {
                    dbContext.Remove(exam);
                    await dbContext.SaveChangesAsync(cancellationToken);
                    await PublishEventAsync(command.ExamId, cancellationToken);
                }
            }

            return await base.HandleAsync(command, cancellationToken);
        }

        private async Task PublishEventAsync(int id, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(
                new ExamDeleted(id),
                cancellationToken);
        }

        private async Task<Domain.Model.ExamModel.Exam> GetAsync(
            ApplicationDbContext dbContext,
            DeleteExamCommand command,
            CancellationToken cancellationToken)
        {
            return await dbContext.Exams.FirstOrDefaultAsync(
                l => l.Id == command.ExamId &&
                     EF.Property<int>(l, "CreatedBy") == command.UserId,
                cancellationToken);
        }
    }
}
