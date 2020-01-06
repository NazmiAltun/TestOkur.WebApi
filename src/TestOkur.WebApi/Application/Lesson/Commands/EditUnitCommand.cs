namespace TestOkur.WebApi.Application.Lesson.Commands
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.CommandsQueries;

    public sealed class EditUnitCommand : CommandBase, IClearCache
    {
        public EditUnitCommand(Guid id, int unitId, string newName)
            : base(id)
        {
            UnitId = unitId;
            NewName = newName;
        }

        public EditUnitCommand()
        {
        }

        public IEnumerable<string> CacheKeys => new[] { $"Units_{UserId}" };

        public int UnitId { get; set; }

        public string NewName { get; set; }
    }
}
