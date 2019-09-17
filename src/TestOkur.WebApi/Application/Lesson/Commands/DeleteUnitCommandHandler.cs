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

    public sealed class DeleteUnitCommandHandler
        : RequestHandlerAsync<DeleteUnitCommand>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteUnitCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [ClearCache(2)]
        public override async Task<DeleteUnitCommand> HandleAsync(
            DeleteUnitCommand command,
            CancellationToken cancellationToken = default)
        {
            var unit = await GetAsync(command, cancellationToken);

            if (unit != null)
            {
                _dbContext.Remove(unit);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return await base.HandleAsync(command, cancellationToken);
        }

        private async Task<Unit> GetAsync(
            DeleteUnitCommand command,
            CancellationToken cancellationToken)
        {
            return await _dbContext.Units.FirstOrDefaultAsync(
                l => l.Id == command.UnitId &&
                     EF.Property<int>(l, "CreatedBy") == command.UserId,
                cancellationToken);
        }
    }
}
