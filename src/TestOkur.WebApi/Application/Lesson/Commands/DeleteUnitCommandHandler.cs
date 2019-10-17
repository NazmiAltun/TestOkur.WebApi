namespace TestOkur.WebApi.Application.Lesson.Commands
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using System.Threading;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.Data;
    using TestOkur.Domain.Model.LessonModel;
    using TestOkur.Infrastructure.CommandsQueries;

    public sealed class DeleteUnitCommandHandler
        : RequestHandlerAsync<DeleteUnitCommand>
    {
        private readonly IApplicationDbContextFactory _dbContextFactory;

        public DeleteUnitCommandHandler(IApplicationDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        [ClearCache(2)]
        public override async Task<DeleteUnitCommand> HandleAsync(
            DeleteUnitCommand command,
            CancellationToken cancellationToken = default)
        {
            using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                var unit = await GetAsync(dbContext, command, cancellationToken);

                if (unit != null)
                {
                    dbContext.Remove(unit);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            }

            return await base.HandleAsync(command, cancellationToken);
        }

        private async Task<Unit> GetAsync(
            ApplicationDbContext dbContext,
            DeleteUnitCommand command,
            CancellationToken cancellationToken)
        {
            var unit = await dbContext.Units.FirstOrDefaultAsync(
                l => l.Id == command.UnitId &&
                     EF.Property<int>(l, "CreatedBy") == command.UserId,
                cancellationToken);

            if (unit.Shared)
            {
                throw new ValidationException(ErrorCodes.CannotApplyAnyOperationOnSharedModels);
            }

            return unit;
        }
    }
}
