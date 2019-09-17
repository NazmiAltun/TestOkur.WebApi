namespace TestOkur.WebApi.Application.Lesson.Commands
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using Paramore.Darker;
    using TestOkur.Common;
    using TestOkur.Data;
    using TestOkur.Domain.Model.LessonModel;
    using TestOkur.Infrastructure.Cqrs;
    using TestOkur.WebApi.Application.Lesson.Queries;

    public sealed class EditUnitCommandHandler : RequestHandlerAsync<EditUnitCommand>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IProcessor _processor;

        public EditUnitCommandHandler(
            ApplicationDbContext dbContext,
            IProcessor processor)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        [Idempotent(2)]
        [ClearCache(4)]
        public override async Task<EditUnitCommand> HandleAsync(
            EditUnitCommand command,
            CancellationToken cancellationToken = default)
        {
            await EnsureNotExistsAsync(command, cancellationToken);
            var unit = await GetAsync(command, cancellationToken);

            if (unit != null)
            {
                unit.SetName(command.NewName);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return await base.HandleAsync(command, cancellationToken);
        }

        private async Task EnsureNotExistsAsync(
            EditUnitCommand command,
            CancellationToken cancellationToken)
        {
            var units = await _processor.ExecuteAsync<GetUserUnitsQuery, IReadOnlyCollection<UnitReadModel>>(
                new GetUserUnitsQuery(),
                cancellationToken);

            if (units.Any(
                c => string.Equals(c.Name, command.NewName, StringComparison.InvariantCultureIgnoreCase) &&
                     c.Id != command.UnitId))
            {
                throw new ValidationException(ErrorCodes.UnitExists);
            }
        }

        private async Task<Unit> GetAsync(
            EditUnitCommand command,
            CancellationToken cancellationToken)
        {
            return await _dbContext.Units.FirstOrDefaultAsync(
                l => l.Id == command.UnitId &&
                     EF.Property<int>(l, "CreatedBy") == command.UserId,
                cancellationToken);
        }
    }
}
