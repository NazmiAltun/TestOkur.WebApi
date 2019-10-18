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
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.WebApi.Application.Lesson.Queries;
    using Lesson = TestOkur.Domain.Model.LessonModel.Lesson;

    public sealed class CreateUnitCommandHandler : RequestHandlerAsync<CreateUnitCommand>
    {
        private readonly IApplicationDbContextFactory _dbContextFactory;
        private readonly IQueryProcessor _queryProcessor;

        public CreateUnitCommandHandler(IApplicationDbContextFactory dbContextFactory, IQueryProcessor queryProcessor)
        {
            _dbContextFactory = dbContextFactory;
            _queryProcessor = queryProcessor;
        }

        [Idempotent(1)]
        [ClearCache(3)]
        public override async Task<CreateUnitCommand> HandleAsync(
            CreateUnitCommand command,
            CancellationToken cancellationToken = default)
        {
            await EnsureUnitDoesNotExistAsync(command, cancellationToken);
            using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                var lesson = await GetLessonAsync(dbContext, command, cancellationToken);
                dbContext.Units.Add(command.ToDomainModel(lesson));
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            return await base.HandleAsync(command, cancellationToken);
        }

        private async Task EnsureUnitDoesNotExistAsync(
            CreateUnitCommand command,
            CancellationToken cancellationToken = default)
        {
            var units = await _queryProcessor.ExecuteAsync(new GetUserUnitsQuery(command.UserId), cancellationToken);

            if (units.Any(l => string.Equals(l.Name, command.Name, StringComparison.InvariantCultureIgnoreCase) &&
                               l.Grade == command.Grade &&
                               l.LessonId == command.LessonId))
            {
                throw new ValidationException(ErrorCodes.UnitExists);
            }
        }

        private async Task<Lesson> GetLessonAsync(
            ApplicationDbContext dbContext,
            CreateUnitCommand command,
            CancellationToken cancellationToken = default)
        {
            return await dbContext.Lessons.FirstAsync(
                l => l.Id == command.LessonId &&
                     (EF.Property<int>(l, "CreatedBy") == command.UserId ||
                      EF.Property<int>(l, "CreatedBy") == default),
                cancellationToken);
        }
    }
}
