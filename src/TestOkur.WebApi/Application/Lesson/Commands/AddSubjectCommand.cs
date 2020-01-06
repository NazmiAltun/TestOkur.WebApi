namespace TestOkur.WebApi.Application.Lesson.Commands
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.CommandsQueries;

    public sealed class AddSubjectCommand : CommandBase, IClearCache
    {
        public AddSubjectCommand(Guid id, string name, int unitId)
         : base(id)
        {
            Name = name;
            UnitId = unitId;
        }

        public AddSubjectCommand()
        {
        }

        public IEnumerable<string> CacheKeys => new[] { $"Units_{UserId}" };

        public string Name { get; set; }

        public int UnitId { get; set; }
    }
}
