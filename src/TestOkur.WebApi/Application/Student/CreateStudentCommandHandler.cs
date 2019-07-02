namespace TestOkur.WebApi.Application.Student
{
	using System;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Microsoft.EntityFrameworkCore;
	using Paramore.Brighter;
	using TestOkur.Data;
	using TestOkur.Infrastructure.Cqrs;
	using Classroom = TestOkur.Domain.Model.ClassroomModel.Classroom;

	public sealed class CreateStudentCommandHandler : RequestHandlerAsync<CreateStudentCommand>
	{
		private readonly ApplicationDbContext _dbContext;

		public CreateStudentCommandHandler(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}

		[Idempotent(1)]
		[Populate(2)]
		[ClearCache(3)]
		public override async Task<CreateStudentCommand> HandleAsync(
			CreateStudentCommand command,
			CancellationToken cancellationToken = default)
		{
			var classroom = await GetClassroomAsync(command.ClassroomId, cancellationToken);
			_dbContext.Students.Add(command.ToDomainModel(classroom));
			_dbContext.AttachRange(command
				.ToDomainModel(classroom)
				.Contacts
				.Select(c => c.ContactType)
				.Distinct());

			await _dbContext.SaveChangesAsync(cancellationToken);

			return await base.HandleAsync(command, cancellationToken);
		}

		private async Task<Classroom> GetClassroomAsync(int id, CancellationToken cancellationToken)
		{
			return await _dbContext.Classrooms.FirstAsync(c => c.Id == id, cancellationToken);
		}
	}
}
