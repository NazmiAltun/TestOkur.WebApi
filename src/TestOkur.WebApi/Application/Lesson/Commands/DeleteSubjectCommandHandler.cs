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
		private readonly ApplicationDbContext _dbContext;

		public DeleteSubjectCommandHandler(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}

		[Populate(1)]
		[ClearCache(2)]
		public override async Task<DeleteSubjectCommand> HandleAsync(
			DeleteSubjectCommand command,
			CancellationToken cancellationToken = default)
		{
			var unit = await GetAsync(command, cancellationToken);

			if (unit != null)
			{
				var subject = unit.RemoveSubject(command.SubjectId);
				_dbContext.Remove(subject);
			}

			await _dbContext.SaveChangesAsync(cancellationToken);
			return await base.HandleAsync(command, cancellationToken);
		}

		private async Task<Unit> GetAsync(
			DeleteSubjectCommand command,
			CancellationToken cancellationToken)
		{
			return await _dbContext.Units
				.Include(u => u.Subjects)
				.FirstOrDefaultAsync(
				l => l.Id == command.UnitId &&
				     EF.Property<int>(l, "CreatedBy") == command.UserId,
				cancellationToken);
		}
	}
}
