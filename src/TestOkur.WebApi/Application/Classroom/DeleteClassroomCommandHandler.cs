namespace TestOkur.WebApi.Application.Classroom
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using TestOkur.Data;
    using TestOkur.Infrastructure.Cqrs;
    using Classroom = TestOkur.Domain.Model.ClassroomModel.Classroom;

    public sealed class DeleteClassroomCommandHandler : RequestHandlerAsync<DeleteClassroomCommand>
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly IPublishEndpoint _publishEndpoint;

		public DeleteClassroomCommandHandler(ApplicationDbContext dbContext, IPublishEndpoint publishEndpoint)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			_publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
		}

		[ClearCache(2)]
		public override async Task<DeleteClassroomCommand> HandleAsync(
			DeleteClassroomCommand command,
			CancellationToken cancellationToken = default)
		{
			var classroom = await GetClassroomAsync(command, cancellationToken);

			if (classroom != null)
			{
				_dbContext.Remove(classroom);
				await _dbContext.SaveChangesAsync(cancellationToken);
				await PublishEventAsync(command.ClassroomId, cancellationToken);
			}

			return await base.HandleAsync(command, cancellationToken);
		}

		private async Task PublishEventAsync(int id, CancellationToken cancellationToken)
		{
			await _publishEndpoint.Publish(
				new ClassroomDeleted(id),
				cancellationToken);
		}

		private async Task<Classroom> GetClassroomAsync(
			DeleteClassroomCommand command,
			CancellationToken cancellationToken)
		{
			return await _dbContext.Classrooms.FirstOrDefaultAsync(
				l => l.Id == command.ClassroomId && EF.Property<int>(l, "CreatedBy") == command.UserId,
				cancellationToken);
		}
	}
}
