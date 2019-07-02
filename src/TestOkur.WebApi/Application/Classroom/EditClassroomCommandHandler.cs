namespace TestOkur.WebApi.Application.Classroom
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using MassTransit;
	using Microsoft.EntityFrameworkCore;
	using Paramore.Brighter;
	using Paramore.Darker;
	using TestOkur.Common;
	using TestOkur.Data;
	using TestOkur.Infrastructure.Cqrs;
	using Classroom = TestOkur.Domain.Model.ClassroomModel.Classroom;

	public sealed class EditClassroomCommandHandler : RequestHandlerAsync<EditClassroomCommand>
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly IQueryProcessor _queryProcessor;
		private readonly IPublishEndpoint _publishEndpoint;

		public EditClassroomCommandHandler(
			ApplicationDbContext dbContext,
			IQueryProcessor queryProcessor,
			IPublishEndpoint publishEndpoint)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			_queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
			_publishEndpoint = publishEndpoint;
		}

		[Idempotent(1)]
		[Populate(2)]
		[ClearCache(3)]
		public override async Task<EditClassroomCommand> HandleAsync(
			EditClassroomCommand command,
			CancellationToken cancellationToken = default)
		{
			await EnsureNotExistsAsync(command, cancellationToken);
			var classroom = await GetClassroomAsync(command, cancellationToken);

			if (classroom != null)
			{
				classroom.Update(command.NewGrade, command.NewName);
				await _dbContext.SaveChangesAsync(cancellationToken);
				await PublishEventAsync(command, cancellationToken);
			}

			return await base.HandleAsync(command, cancellationToken);
		}

		private async Task PublishEventAsync(EditClassroomCommand command, CancellationToken cancellationToken)
		{
			await _publishEndpoint.Publish(
				new ClassroomUpdated(command.ClassroomId, command.NewGrade, command.NewName),
				cancellationToken);
		}

		private async Task EnsureNotExistsAsync(
			EditClassroomCommand command,
			CancellationToken cancellationToken)
		{
			var classrooms = await _queryProcessor.ExecuteAsync(
				new GetUserClassroomsQuery(),
				cancellationToken);

			if (classrooms.Any(
				c => c.Grade == command.NewGrade &&
					 string.Equals(c.Name, command.NewName, StringComparison.InvariantCultureIgnoreCase) &&
					 c.Id != command.ClassroomId))
			{
				throw new ValidationException(ErrorCodes.ClassroomExists);
			}
		}

		private async Task<Classroom> GetClassroomAsync(
			EditClassroomCommand command,
			CancellationToken cancellationToken)
		{
			return await _dbContext.Classrooms.FirstOrDefaultAsync(
				l => l.Id == command.ClassroomId && EF.Property<int>(l, "CreatedBy") == command.UserId,
				cancellationToken);
		}
	}
}
