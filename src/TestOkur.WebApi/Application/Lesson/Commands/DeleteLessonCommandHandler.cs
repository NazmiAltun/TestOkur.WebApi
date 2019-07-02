namespace TestOkur.WebApi.Application.Lesson.Commands
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using Microsoft.EntityFrameworkCore;
	using Paramore.Brighter;
	using TestOkur.Data;
	using TestOkur.Infrastructure.Cqrs;
	using Lesson = TestOkur.Domain.Model.LessonModel.Lesson;

	public sealed class DeleteLessonCommandHandler : RequestHandlerAsync<DeleteLessonCommand>
	{
		private readonly ApplicationDbContext _dbContext;

		public DeleteLessonCommandHandler(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}

		[Populate(1)]
		[ClearCache(2)]
		public override async Task<DeleteLessonCommand> HandleAsync(
			DeleteLessonCommand command,
			CancellationToken cancellationToken = default)
		{
			var lesson = await GetAsync(command, cancellationToken);

			if (lesson != null)
			{
				_dbContext.Remove(lesson);
				await _dbContext.SaveChangesAsync(cancellationToken);
			}

			return await base.HandleAsync(command, cancellationToken);
		}

		private async Task<Lesson> GetAsync(
			DeleteLessonCommand command,
			CancellationToken cancellationToken)
		{
			return await _dbContext.Lessons.FirstOrDefaultAsync(
				l => l.Id == command.LessonId &&
				     EF.Property<int>(l, "CreatedBy") == command.UserId,
				cancellationToken);
		}
	}
}
