namespace TestOkur.WebApi.Application.Student
{
    using System.Collections.Generic;
    using TestOkur.Infrastructure.Cqrs;

    public sealed class DeleteStudentCommand : CommandBase, IClearCache
    {
        public DeleteStudentCommand(int studentId)
        {
            StudentId = studentId;
        }

        public IEnumerable<string> CacheKeys => new[]
        {
            $"Students_{UserId}",
            $"Contacts_{UserId}",
        };

        public int StudentId { get; }
    }
}
