namespace TestOkur.WebApi.Application.Lesson.Commands
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using Paramore.Darker;
    using TestOkur.Common;
    using TestOkur.Data;
    using TestOkur.Infrastructure.Cqrs;
    using TestOkur.WebApi.Application.Lesson.Queries;
    using Lesson = TestOkur.Domain.Model.LessonModel.Lesson;

    public sealed class CreateUnitCommandHandler : RequestHandlerAsync<CreateUnitCommand>
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly IQueryProcessor _queryProcessor;

		public CreateUnitCommandHandler(
			ApplicationDbContext dbContext,
			IQueryProcessor queryProcessor)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			_queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
		}

		[Idempotent(1)]
		[ClearCache(3)]
		public override async Task<CreateUnitCommand> HandleAsync(
			CreateUnitCommand command,
			CancellationToken cancellationToken = default)
		{
			await EnsureUnitDoesNotExistAsync(command, cancellationToken);
			var lesson = await GetLessonAsync(command, cancellationToken);
			_dbContext.Units.Add(command.ToDomainModel(lesson));
			await _dbContext.SaveChangesAsync(cancellationToken);

			return await base.HandleAsync(command, cancellationToken);
		}

		private async Task EnsureUnitDoesNotExistAsync(
			CreateUnitCommand command,
			CancellationToken cancellationToken = default)
		{
			var units = await _queryProcessor.ExecuteAsync(
				new GetUserUnitsQuery(), cancellationToken);

			if (units.Any(l => string.Equals(l.Name, command.Name, StringComparison.InvariantCultureIgnoreCase) &&
							   l.Grade == command.Grade &&
							   l.LessonId == command.LessonId))
			{
				throw new ValidationException(ErrorCodes.UnitExists);
			}
		}

		private async Task<Lesson> GetLessonAsync(
			CreateUnitCommand command,
			CancellationToken cancellationToken = default)
		{
			return await _dbContext.Lessons.FirstAsync(
				l => l.Id == command.LessonId &&
					 (EF.Property<int>(l, "CreatedBy") == command.UserId ||
					  EF.Property<int>(l, "CreatedBy") == default),
				cancellationToken);
		}
	}
}
