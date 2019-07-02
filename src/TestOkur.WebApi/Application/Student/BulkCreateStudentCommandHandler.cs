namespace TestOkur.WebApi.Application.Student
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Microsoft.EntityFrameworkCore;
	using Paramore.Brighter;
	using TestOkur.Data;
	using TestOkur.Domain.Model.StudentModel;
	using TestOkur.Infrastructure.Cqrs;
	using Classroom = TestOkur.Domain.Model.ClassroomModel.Classroom;

	public class BulkCreateStudentCommandHandler : RequestHandlerAsync<BulkCreateStudentCommand>
	{
		private readonly ApplicationDbContext _dbContext;

		public BulkCreateStudentCommandHandler(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}

		[Idempotent(1)]
		[Populate(2)]
		[ClearCache(3)]
		public override async Task<BulkCreateStudentCommand> HandleAsync(
			BulkCreateStudentCommand command,
			CancellationToken cancellationToken = default)
		{
			var classroom = await GetClassroomAsync(command, cancellationToken);
			var contactTypes = new List<ContactType>();

			foreach (var subCommand in command.Commands)
			{
				_dbContext.Students.Add(subCommand.ToDomainModel(classroom, command.UserId));
				contactTypes.AddRange(subCommand
					.ToDomainModel(classroom, command.UserId)
					.Contacts.Select(c => c.ContactType));
			}

			_dbContext.AttachRange(contactTypes.Distinct());
			await _dbContext.SaveChangesAsync(cancellationToken);

			return await base.HandleAsync(command, cancellationToken);
		}

		private async Task<Classroom> GetClassroomAsync(
			BulkCreateStudentCommand command,
			CancellationToken cancellationToken)
		{
			return await _dbContext.Classrooms
				.FirstAsync(
					c => c.Id == command.Commands.First().ClassroomId,
					cancellationToken);
		}
	}
}
