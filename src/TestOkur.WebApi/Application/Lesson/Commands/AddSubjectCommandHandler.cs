namespace TestOkur.WebApi.Application.Lesson.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using TestOkur.Data;
    using TestOkur.Domain.Model.LessonModel;
    using TestOkur.Infrastructure.CommandsQueries;

    public class AddSubjectCommandHandler : RequestHandlerAsync<AddSubjectCommand>
    {
        private readonly IApplicationDbContextFactory _dbContextFactory;

        public AddSubjectCommandHandler(IApplicationDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        [Idempotent(1)]
        [ClearCache(3)]
        public override async Task<AddSubjectCommand> HandleAsync(
            AddSubjectCommand command,
            CancellationToken cancellationToken = default)
        {
            using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                var unit = await GetUnitAsync(dbContext, command, cancellationToken);
                unit.AddSubject(command.Name);
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            return await base.HandleAsync(command, cancellationToken);
        }

        private async Task<Unit> GetUnitAsync(
            ApplicationDbContext dbContext,
            AddSubjectCommand command,
            CancellationToken cancellationToken)
        {
            return await dbContext.Units
                .Include(u => u.Subjects)
                .FirstAsync(
                    u => u.Id == command.UnitId &&
                         EF.Property<int>(u, "CreatedBy") == command.UserId,
                    cancellationToken);
        }
    }
}
