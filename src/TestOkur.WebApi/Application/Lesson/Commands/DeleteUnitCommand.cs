namespace TestOkur.WebApi.Application.Lesson.Commands
{
    using System.Collections.Generic;
    using TestOkur.Infrastructure.Cqrs;

    public sealed class DeleteUnitCommand : CommandBase, IClearCache
    {
        public DeleteUnitCommand(int unitId)
        {
            UnitId = unitId;
        }

        public IEnumerable<string> CacheKeys => new[] { $"Units_{UserId}" };

        public int UnitId { get; }
    }
}
