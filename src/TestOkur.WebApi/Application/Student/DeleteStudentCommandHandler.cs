namespace TestOkur.WebApi.Application.Student
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using MassTransit;
	using Microsoft.EntityFrameworkCore;
	using Paramore.Brighter;
	using TestOkur.Data;
	using TestOkur.Infrastructure.Cqrs;
	using Student = TestOkur.Domain.Model.StudentModel.Student;

	public class DeleteStudentCommandHandler : RequestHandlerAsync<DeleteStudentCommand>
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly IPublishEndpoint _publishEndpoint;

		public DeleteStudentCommandHandler(ApplicationDbContext dbContext, IPublishEndpoint publishEndpoint)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			_publishEndpoint = publishEndpoint;
		}

		[Populate(1)]
		[ClearCache(2)]
		public override async Task<DeleteStudentCommand> HandleAsync(
			DeleteStudentCommand command,
			CancellationToken cancellationToken = default)
		{
			var student = await GetStudentAsync(command, cancellationToken);

			if (student != null)
			{
				_dbContext.Remove(student);
				await _dbContext.SaveChangesAsync(cancellationToken);
				await PublishEventAsync(command.StudentId, cancellationToken);
			}

			return await base.HandleAsync(command, cancellationToken);
		}

		private async Task PublishEventAsync(int id, CancellationToken cancellationToken)
		{
			await _publishEndpoint.Publish(
				new StudentDeleted(id),
				cancellationToken);
		}

		private async Task<Student> GetStudentAsync(
			DeleteStudentCommand command,
			CancellationToken cancellationToken)
		{
			return await _dbContext.Students.FirstOrDefaultAsync(
				l => l.Id == command.StudentId &&
				     EF.Property<int>(l, "CreatedBy") == command.UserId,
				cancellationToken);
		}
	}
}
