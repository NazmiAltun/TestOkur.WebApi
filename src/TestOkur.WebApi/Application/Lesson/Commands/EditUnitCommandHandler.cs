namespace TestOkur.WebApi.Application.Lesson.Commands
{
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using Paramore.Darker;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.Data;
    using TestOkur.Domain.Model.LessonModel;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.WebApi.Application.Lesson.Queries;

    public sealed class EditUnitCommandHandler : RequestHandlerAsync<EditUnitCommand>
    {
        private readonly IApplicationDbContextFactory _dbContextFactory;
        private readonly IQueryProcessor _queryProcessor;

        public EditUnitCommandHandler(IApplicationDbContextFactory dbContextFactory, IQueryProcessor queryProcessor)
        {
            _dbContextFactory = dbContextFactory;
            _queryProcessor = queryProcessor;
        }

        [Idempotent(2)]
        [ClearCache(4)]
        public override async Task<EditUnitCommand> HandleAsync(
            EditUnitCommand command,
            CancellationToken cancellationToken = default)
        {
            await EnsureNotExistsAsync(command, cancellationToken);
            await using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                var unit = await GetAsync(dbContext, command, cancellationToken);

                if (unit != null)
                {
                    unit.SetName(command.NewName);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            }

            return await base.HandleAsync(command, cancellationToken);
        }

        private async Task EnsureNotExistsAsync(
            EditUnitCommand command,
            CancellationToken cancellationToken)
        {
            var units = await _queryProcessor.ExecuteAsync(new GetUserUnitsQuery(command.UserId), cancellationToken);

            if (units.Any(
                c => string.Equals(c.Name, command.NewName, StringComparison.InvariantCultureIgnoreCase) &&
                     c.Id != command.UnitId))
            {
                throw new ValidationException(ErrorCodes.UnitExists);
            }
        }

        private async Task<Unit> GetAsync(
            ApplicationDbContext dbContext,
            EditUnitCommand command,
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
