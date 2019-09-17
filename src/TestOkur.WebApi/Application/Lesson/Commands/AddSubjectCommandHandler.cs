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

    public class AddSubjectCommandHandler : RequestHandlerAsync<AddSubjectCommand>
	{
		private readonly ApplicationDbContext _dbContext;

		public AddSubjectCommandHandler(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}

		[Idempotent(1)]
		[ClearCache(3)]
		public override async Task<AddSubjectCommand> HandleAsync(
			AddSubjectCommand command,
			CancellationToken cancellationToken = default)
		{
			var unit = await GetUnitAsync(command, cancellationToken);
			unit.AddSubject(command.Name);
			await _dbContext.SaveChangesAsync(cancellationToken);
			return await base.HandleAsync(command, cancellationToken);
		}

		private async Task<Unit> GetUnitAsync(
			AddSubjectCommand command,
			CancellationToken cancellationToken)
		{
			return await _dbContext.Units
				.Include(u => u.Subjects)
				.FirstAsync(
					u => u.Id == command.UnitId &&
						 EF.Property<int>(u, "CreatedBy") == command.UserId,
					cancellationToken);
		}
	}
}
