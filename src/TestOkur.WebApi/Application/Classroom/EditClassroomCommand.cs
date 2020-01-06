namespace TestOkur.WebApi.Application.Classroom
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.CommandsQueries;

    public sealed class EditClassroomCommand : CommandBase, IClearCache
    {
        public EditClassroomCommand(Guid id, int classroomId, string newName, int newGrade)
            : base(id)
        {
            ClassroomId = classroomId;
            NewName = newName;
            NewGrade = newGrade;
        }

        public EditClassroomCommand()
        {
        }

        public IEnumerable<string> CacheKeys => new[]
        {
            $"Classrooms_{UserId}",
            $"Students_{UserId}",
            $"Contacts_{UserId}",
        };

        public int ClassroomId { get; set; }

        public string NewName { get; set; }

        public int NewGrade { get; set; }
    }
}
