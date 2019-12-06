namespace TestOkur.WebApi.Application.Student
{
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using Paramore.Darker;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.Data;
    using TestOkur.Domain.Model;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.WebApi.Application.Classroom;

    public sealed class CreateStudentCommandHandler : RequestHandlerAsync<CreateStudentCommand>
    {
        private readonly IApplicationDbContextFactory _dbContextFactory;
        private readonly IQueryProcessor _queryProcessor;

        public CreateStudentCommandHandler(IApplicationDbContextFactory dbContextFactory, IQueryProcessor queryProcessor)
        {
            _dbContextFactory = dbContextFactory;
            _queryProcessor = queryProcessor;
        }

        [Idempotent(1)]
        [ClearCache(3)]
        public override async Task<CreateStudentCommand> HandleAsync(
            CreateStudentCommand command,
            CancellationToken cancellationToken = default)
        {
            await EnsureStudentDoesNotExists(command, cancellationToken);
            await using (var dbContext = _dbContextFactory.Create(command.UserId))
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
            var getUserStudentsQuery = new GetUserStudentsQuery(command.UserId);
            var studentList = await _queryProcessor.ExecuteAsync(getUserStudentsQuery, cancellationToken);
            var existingStudent = studentList.FirstOrDefault(x => x.StudentNumber == command.StudentNumber);

            if (existingStudent != null)
            {
                var getUserClassroomsQuery = new GetUserClassroomsQuery(command.UserId);
                var classroomList = await _queryProcessor.ExecuteAsync(getUserClassroomsQuery, cancellationToken);
                var newStudentGrade = classroomList.First(c => c.Id == command.ClassroomId).Grade;

                if ((Grade.CheckIfHighSchool(existingStudent.ClassroomGrade) &&
                    Grade.CheckIfHighSchool(newStudentGrade)) ||
                    (Grade.CheckIfSecondarySchool(existingStudent.ClassroomGrade) && Grade.CheckIfSecondarySchool(newStudentGrade)))
                {
                    throw new ValidationException(ErrorCodes.StudentExists);
                }
            }
        }
    }
}
