namespace TestOkur.WebApi.Application.Lesson.Commands
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using TestOkur.Data;
    using TestOkur.Domain.Model.LessonModel;
    using TestOkur.Infrastructure.Cqrs;

    public sealed class EditSubjectCommandHandler
		: RequestHandlerAsync<EditSubjectCommand>
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly IPublishEndpoint _publishEndpoint;

		public EditSubjectCommandHandler(
			ApplicationDbContext dbContext,
			IPublishEndpoint publishEndpoint)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			_publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
		}

		[Idempotent(1)]
		[ClearCache(3)]
		public override async Task<EditSubjectCommand> HandleAsync(
			EditSubjectCommand command,
			CancellationToken cancellationToken = default)
		{
			var unit = await GetAsync(command, cancellationToken);
			unit.Subjects.First(s => s.Id == command.SubjectId)
				.SetName(command.NewName);
			await _dbContext.SaveChangesAsync(cancellationToken);
			await PublishEventAsync(command, cancellationToken);
			return await base.HandleAsync(command, cancellationToken);
		}

		private async Task PublishEventAsync(
			EditSubjectCommand command,
			CancellationToken cancellationToken)
		{
			await _publishEndpoint.Publish(
				new SubjectChanged(command.SubjectId, command.NewName),
				cancellationToken);
		}

		private async Task<Unit> GetAsync(
			EditSubjectCommand command,
			CancellationToken cancellationToken)
		{
			return await _dbContext.Units
				.Include(u => u.Subjects)
				.FirstOrDefaultAsync(
					u => u.Id == command.UnitId &&
					     EF.Property<int>(u, "CreatedBy") == command.UserId,
					cancellationToken);
		}
	}
}
