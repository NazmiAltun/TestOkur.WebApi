namespace TestOkur.WebApi.Application.Student
{
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
    using TestOkur.Infrastructure.CommandsQueries;
    using Classroom = TestOkur.Domain.Model.ClassroomModel.Classroom;
    using Student = TestOkur.Domain.Model.StudentModel.Student;

    public sealed class EditStudentCommandHandler : RequestHandlerAsync<EditStudentCommand>
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly IApplicationDbContextFactory _dbContextFactory;
        private readonly IPublishEndpoint _publishEndpoint;

        public EditStudentCommandHandler(IPublishEndpoint publishEndpoint, IApplicationDbContextFactory dbContextFactory, IQueryProcessor queryProcessor)
        {
            _publishEndpoint = publishEndpoint;
            _dbContextFactory = dbContextFactory;
            _queryProcessor = queryProcessor;
        }

        [Idempotent(1)]
        [ClearCache(3)]
        public override async Task<EditStudentCommand> HandleAsync(
            EditStudentCommand command,
            CancellationToken cancellationToken = default)
        {
            await using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                await EnsureNotExistsAsync(command, cancellationToken);
                var student = await GetStudentAsync(dbContext, command, cancellationToken);

                if (student != null)
                {
                    dbContext.RemoveRange(student.Contacts);
                    student.Update(
                        command.NewFirstName,
                        command.NewLastName,
                        command.NewStudentNumber,
                        await GetClassroomAsync(dbContext, command, cancellationToken),
                        command.Contacts?.Select(c => c.ToDomainModel()),
                        command.CitizenshipIdentity,
                        command.NewNotes);
                    dbContext.AttachRange(student.Contacts.Select(c => c.ContactType));
                    await dbContext.SaveChangesAsync(cancellationToken);
                    await PublishEventAsync(command, cancellationToken);
                }
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

        private Task<Classroom> GetClassroomAsync(
            ApplicationDbContext dbContext,
            EditStudentCommand command,
            CancellationToken cancellationToken)
        {
            return dbContext.Classrooms.FirstAsync(
                l => l.Id == command.NewClassroomId &&
                     EF.Property<int>(l, "CreatedBy") == command.UserId,
                cancellationToken);
        }

        private Task<Student> GetStudentAsync(
            ApplicationDbContext dbContext,
            EditStudentCommand command,
            CancellationToken cancellationToken)
        {
            return dbContext.Students
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
            var students = await _queryProcessor.ExecuteAsync(
                    new GetUserStudentsQuery(command.UserId), cancellationToken);
            if (students.Any(
                s => s.StudentNumber == command.NewStudentNumber &&
                     s.Id != command.StudentId))
            {
                throw new ValidationException(ErrorCodes.StudentExists);
            }
        }
    }
}
