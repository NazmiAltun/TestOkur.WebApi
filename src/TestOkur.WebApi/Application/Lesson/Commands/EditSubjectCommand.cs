namespace TestOkur.WebApi.Application.Lesson.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TestOkur.Infrastructure.CommandsQueries;

    [DataContract]
    public sealed class EditSubjectCommand : CommandBase, IClearCache
    {
        public EditSubjectCommand(Guid id, int subjectId, string newName)
            : base(id)
        {
            SubjectId = subjectId;
            NewName = newName;
        }

        public IEnumerable<string> CacheKeys => new[] { $"Units_{UserId}" };

        public int UnitId { get; set; }

        [DataMember]
        public int SubjectId { get; private set; }

        [DataMember]
        public string NewName { get; private set; }
    }
}
