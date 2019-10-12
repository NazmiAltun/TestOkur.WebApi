namespace TestOkur.WebApi.Application.Student
{
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.Data;
    using TestOkur.Infrastructure.CommandsQueries;

    public sealed class CreateStudentCommandHandler : RequestHandlerAsync<CreateStudentCommand>
    {
        private readonly IApplicationDbContextFactory _dbContextFactory;
        private readonly IProcessor _processor;

        public CreateStudentCommandHandler(IProcessor processor, IApplicationDbContextFactory dbContextFactory)
        {
            _processor = processor;
            _dbContextFactory = dbContextFactory;
        }

        [Idempotent(1)]
        [ClearCache(3)]
        public override async Task<CreateStudentCommand> HandleAsync(
            CreateStudentCommand command,
            CancellationToken cancellationToken = default)
        {
            await EnsureStudentDoesNotExists(command, cancellationToken);
            using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                var classroom = await dbContext.Classrooms.FirstAsync(c => c.Id == command.ClassroomId, cancellationToken);
                dbContext.Students.Add(command.ToDomainModel(classroom));
                dbContext.AttachRange(command
                    .ToDomainModel(classroom)
                    .Contacts
                    .Select(c => c.ContactType)
                    .Distinct());

                await dbContext.SaveChangesAsync(cancellationToken);
            }

            return await base.HandleAsync(command, cancellationToken);
        }

        private async Task EnsureStudentDoesNotExists(
            CreateStudentCommand command,
            CancellationToken cancellationToken)
        {
            var query = new GetUserStudentsQuery(command.UserId);
            var list = await _processor.ExecuteAsync<GetUserStudentsQuery, IReadOnlyCollection<StudentReadModel>>(query, cancellationToken);

            if (list.Any(c => c.StudentNumber == command.StudentNumber))
            {
                throw new ValidationException(ErrorCodes.StudentExists);
            }
        }
    }
}
