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
        private readonly IApplicationDbContextFactory _dbContextFactory;
        private readonly IPublishEndpoint _publishEndpoint;

        public EditStudentCommandHandler(IPublishEndpoint publishEndpoint, IApplicationDbContextFactory dbContextFactory)
        {
            _publishEndpoint = publishEndpoint;
            _dbContextFactory = dbContextFactory;
        }

        [Idempotent(1)]
        [ClearCache(3)]
        public override async Task<EditStudentCommand> HandleAsync(
            EditStudentCommand command,
            CancellationToken cancellationToken = default)
        {
            using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                await EnsureNotExistsAsync(dbContext, command, cancellationToken);
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

        private async Task<Classroom> GetClassroomAsync(
            ApplicationDbContext dbContext,
            EditStudentCommand command,
            CancellationToken cancellationToken)
        {
            return await dbContext.Classrooms.FirstAsync(
                l => l.Id == command.NewClassroomId &&
                     EF.Property<int>(l, "CreatedBy") == command.UserId,
                cancellationToken);
        }

        private async Task<Student> GetStudentAsync(
            ApplicationDbContext dbContext,
            EditStudentCommand command,
            CancellationToken cancellationToken)
        {
            return await dbContext.Students
                .Include(s => s.Contacts)
                .FirstOrDefaultAsync(
                    l => l.Id == command.StudentId &&
                         EF.Property<int>(l, "CreatedBy") == command.UserId,
                    cancellationToken);
        }

        //TODO:Use query processor instead
        private async Task EnsureNotExistsAsync(
            ApplicationDbContext dbContext,
            EditStudentCommand command,
            CancellationToken cancellationToken)
        {
            if (await dbContext.Students.AnyAsync(
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
