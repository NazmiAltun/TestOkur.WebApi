namespace TestOkur.WebApi.Application.Student
{
    using System.Collections.Generic;
    using TestOkur.Infrastructure.CommandsQueries;

    public sealed class DeleteStudentCommand : CommandBase, IClearCache
    {
        public DeleteStudentCommand(int studentId)
        {
            StudentId = studentId;
        }

        public DeleteStudentCommand()
        {
        }

        public IEnumerable<string> CacheKeys => new[]
        {
            $"Students_{UserId}",
            $"Contacts_{UserId}",
        };

        public int StudentId { get; }
    }
}
