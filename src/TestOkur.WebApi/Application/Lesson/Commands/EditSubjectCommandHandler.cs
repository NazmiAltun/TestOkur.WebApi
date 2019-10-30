namespace TestOkur.WebApi.Application.Lesson.Commands
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using TestOkur.Common;
    using TestOkur.Data;
    using TestOkur.Domain.Model.LessonModel;
    using TestOkur.Infrastructure.CommandsQueries;

    public sealed class EditSubjectCommandHandler
        : RequestHandlerAsync<EditSubjectCommand>
    {
        private readonly IApplicationDbContextFactory _dbContextFactory;
        private readonly IPublishEndpoint _publishEndpoint;

        public EditSubjectCommandHandler(
            IPublishEndpoint publishEndpoint,
            IApplicationDbContextFactory dbContextFactory)
        {
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            _dbContextFactory = dbContextFactory;
        }

        [Idempotent(1)]
        [ClearCache(3)]
        public override async Task<EditSubjectCommand> HandleAsync(
            EditSubjectCommand command,
            CancellationToken cancellationToken = default)
        {
            await using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                var unit = await GetAsync(dbContext, command, cancellationToken);
                unit.Subjects.First(s => s.Id == command.SubjectId)
                    .SetName(command.NewName);
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            await PublishEventAsync(command, cancellationToken);
            return await base.HandleAsync(command, cancellationToken);
        }

        private async Task PublishEventAsync(
            EditSubjectCommand command,
            CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(
                new SubjectChanged(command.SubjectId, command.NewName),
                cancellationToken);
        }

        private async Task<Unit> GetAsync(
            ApplicationDbContext dbContext,
            EditSubjectCommand command,
            CancellationToken cancellationToken)
        {
            var unit = await dbContext.Units
                .Include(u => u.Subjects)
                .FirstOrDefaultAsync(
                    u => u.Id == command.UnitId &&
                         EF.Property<int>(u, "CreatedBy") == command.UserId,
                    cancellationToken);

            if (unit.Shared)
            {
                throw new ValidationException(ErrorCodes.CannotApplyAnyOperationOnSharedModels);
            }

            return unit;
        }
    }
}
