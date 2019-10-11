namespace TestOkur.WebApi.Application.Lesson.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TestOkur.Infrastructure.CommandsQueries;

    [DataContract]
    public sealed class EditUnitCommand : CommandBase, IClearCache
    {
        public EditUnitCommand(Guid id, int unitId, string newName)
            : base(id)
        {
            UnitId = unitId;
            NewName = newName;
        }

        public IEnumerable<string> CacheKeys => new[] { $"Units_{UserId}" };

        [DataMember]
        public int UnitId { get; private set; }

        [DataMember]
        public string NewName { get; private set; }
    }
}
