namespace TestOkur.WebApi.Application.Classroom
{
    using System.Collections.Generic;
    using TestOkur.Infrastructure.Cqrs;

    public sealed class DeleteClassroomCommand : CommandBase, IClearCache
    {
        public DeleteClassroomCommand(int classroomId)
        {
            ClassroomId = classroomId;
        }

        public int ClassroomId { get; }

        public IEnumerable<string> CacheKeys => new[]
        {
            $"Classrooms_{UserId}",
            $"Students_{UserId}",
            $"Contacts_{UserId}",
        };
    }
}
