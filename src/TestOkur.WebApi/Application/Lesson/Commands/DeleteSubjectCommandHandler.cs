namespace TestOkur.WebApi.Application.Lesson.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using TestOkur.Data;
    using TestOkur.Domain.Model.LessonModel;
    using TestOkur.Infrastructure.Cqrs;

    public sealed class DeleteSubjectCommandHandler
        : RequestHandlerAsync<DeleteSubjectCommand>
    {
        private readonly IApplicationDbContextFactory _dbContextFactory;

        public DeleteSubjectCommandHandler(IApplicationDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        [ClearCache(2)]
        public override async Task<DeleteSubjectCommand> HandleAsync(
            DeleteSubjectCommand command,
            CancellationToken cancellationToken = default)
        {
            using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                var unit = await GetAsync(dbContext, command, cancellationToken);

                if (unit != null)
                {
                    var subject = unit.RemoveSubject(command.SubjectId);
                    dbContext.Remove(subject);
                }

                await dbContext.SaveChangesAsync(cancellationToken);
            }

            return await base.HandleAsync(command, cancellationToken);
        }

        private async Task<Unit> GetAsync(
            ApplicationDbContext dbContext,
            DeleteSubjectCommand command,
            CancellationToken cancellationToken)
        {
            return await dbContext.Units
                .Include(u => u.Subjects)
                .FirstOrDefaultAsync(
                l => l.Id == command.UnitId &&
                     EF.Property<int>(l, "CreatedBy") == command.UserId,
                cancellationToken);
        }
    }
}
