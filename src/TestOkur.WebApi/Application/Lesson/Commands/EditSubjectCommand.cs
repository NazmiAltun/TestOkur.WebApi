namespace TestOkur.WebApi.Application.Lesson.Commands
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.CommandsQueries;

    public sealed class EditSubjectCommand : CommandBase, IClearCache
    {
        public EditSubjectCommand(Guid id, int subjectId, string newName)
            : base(id)
        {
            SubjectId = subjectId;
            NewName = newName;
        }

        public EditSubjectCommand()
        {
        }

        public IEnumerable<string> CacheKeys => new[] { $"Units_{UserId}" };

        public int UnitId { get; set; }

        public int SubjectId { get; set; }

        public string NewName { get; set; }
    }
}
