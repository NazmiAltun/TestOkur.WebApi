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
            using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                foreach (var subCommand in command.Commands)
                {
                    var classroom = await GetClassroomAsync(dbContext, subCommand.ClassroomId, cancellationToken);
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

        private async Task<Classroom> GetClassroomAsync(
            ApplicationDbContext dbContext,
            int classroomId,
            CancellationToken cancellationToken)
        {
            return await dbContext.Classrooms
                .FirstAsync(c => c.Id == classroomId, cancellationToken);
        }
    }
}
