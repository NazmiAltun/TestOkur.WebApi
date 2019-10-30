namespace TestOkur.WebApi.Application.Student
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using TestOkur.Data;
    using TestOkur.Domain.Model.StudentModel;
    using TestOkur.Infrastructure.CommandsQueries;
    using Classroom = TestOkur.Domain.Model.ClassroomModel.Classroom;

    public class BulkCreateStudentCommandHandler : RequestHandlerAsync<BulkCreateStudentCommand>
    {
        private readonly IApplicationDbContextFactory _dbContextFactory;

        public BulkCreateStudentCommandHandler(IApplicationDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        [Idempotent(1)]
        [ClearCache(3)]
        public override async Task<BulkCreateStudentCommand> HandleAsync(
            BulkCreateStudentCommand command,
            CancellationToken cancellationToken = default)
        {
            var contactTypes = new List<ContactType>();
            await using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                foreach (var subCommand in command.Commands)
                {
                    var classroom = await GetClassroomAsync(dbContext, subCommand.ClassroomId, cancellationToken);
                    var existingStudent = await dbContext.Students
                        .FirstOrDefaultAsync(
                            s => s.StudentNumber.Value == subCommand.StudentNumber &&
                                 EF.Property<int>(s, "CreatedBy") == command.UserId,
                            cancellationToken);
                    if (existingStudent != null)
                    {
                        dbContext.Remove(existingStudent);
                    }

                    dbContext.Students.Add(subCommand.ToDomainModel(classroom, command.UserId));
                    contactTypes.AddRange(subCommand
                        .ToDomainModel(classroom, command.UserId)
                        .Contacts.Select(c => c.ContactType));
                }

                dbContext.AttachRange(contactTypes.Distinct());
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            return await base.HandleAsync(command, cancellationToken);
        }

        private Task<Classroom> GetClassroomAsync(
            ApplicationDbContext dbContext,
            int classroomId,
            CancellationToken cancellationToken)
        {
            return dbContext.Classrooms
                .FirstAsync(c => c.Id == classroomId, cancellationToken);
        }
    }
}
