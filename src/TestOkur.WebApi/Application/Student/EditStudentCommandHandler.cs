namespace TestOkur.WebApi.Application.Student
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using MassTransit;
	using Microsoft.EntityFrameworkCore;
	using Paramore.Brighter;
	using TestOkur.Common;
	using TestOkur.Data;
	using TestOkur.Infrastructure.Cqrs;
	using Classroom = TestOkur.Domain.Model.ClassroomModel.Classroom;
	using Student = TestOkur.Domain.Model.StudentModel.Student;

	public sealed class EditStudentCommandHandler : RequestHandlerAsync<EditStudentCommand>
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly IPublishEndpoint _publishEndpoint;

		public EditStudentCommandHandler(ApplicationDbContext dbContext, IPublishEndpoint publishEndpoint)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			_publishEndpoint = publishEndpoint;
		}

		[Idempotent(1)]
		[Populate(2)]
		[ClearCache(3)]
		public override async Task<EditStudentCommand> HandleAsync(
			EditStudentCommand command,
			CancellationToken cancellationToken = default)
		{
			await EnsureNotExistsAsync(command, cancellationToken);
			var student = await GetStudentAsync(command, cancellationToken);

			if (student != null)
			{
				_dbContext.RemoveRange(student.Contacts);
				student.Update(
					command.NewFirstName,
					command.NewLastName,
					command.NewStudentNumber,
					await GetClassroomAsync(command, cancellationToken),
					command.Contacts?.Select(c => c.ToDomainModel()),
					command.NewNotes);
				_dbContext.AttachRange(student.Contacts.Select(c => c.ContactType));
				await _dbContext.SaveChangesAsync(cancellationToken);
				await PublishEventAsync(command, cancellationToken);
			}

			return await base.HandleAsync(command, cancellationToken);
		}

		private async Task PublishEventAsync(EditStudentCommand command, CancellationToken cancellationToken)
		{
			await _publishEndpoint.Publish(
				new StudentUpdated(
					command.NewClassroomId,
					command.NewStudentNumber,
					command.NewLastName,
					command.NewFirstName,
					command.StudentId),
				cancellationToken);
		}

		private async Task<Classroom> GetClassroomAsync(
			EditStudentCommand command,
			CancellationToken cancellationToken)
		{
			return await _dbContext.Classrooms.FirstAsync(
				l => l.Id == command.NewClassroomId &&
				     EF.Property<int>(l, "CreatedBy") == command.UserId,
				cancellationToken);
		}

		private async Task<Student> GetStudentAsync(
			EditStudentCommand command,
			CancellationToken cancellationToken)
		{
			return await _dbContext.Students
				.Include(s => s.Contacts)
				.FirstOrDefaultAsync(
					l => l.Id == command.StudentId &&
					     EF.Property<int>(l, "CreatedBy") == command.UserId,
					cancellationToken);
		}

		private async Task EnsureNotExistsAsync(
			EditStudentCommand command,
			CancellationToken cancellationToken)
		{
			if (await _dbContext.Students.AnyAsync(
				c => c.StudentNumber.Value == command.NewStudentNumber &&
					 c.Id != command.StudentId &&
					 EF.Property<int>(c, "CreatedBy") == command.UserId,
				cancellationToken))
			{
				throw new ValidationException(ErrorCodes.StudentExists);
			}
		}
	}
}
