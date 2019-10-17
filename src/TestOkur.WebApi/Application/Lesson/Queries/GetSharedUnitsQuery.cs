namespace TestOkur.WebApi.Application.Lesson.Queries
{
    using System;
    using System.Collections.Generic;
    using Paramore.Darker;
    using TestOkur.Infrastructure.CommandsQueries;

    public sealed class GetSharedUnitsQuery :
        IQuery<IReadOnlyCollection<UnitReadModel>>,
        ICacheResult
    {
        public string CacheKey => "SharedUnits";

        public TimeSpan CacheDuration => TimeSpan.FromDays(1);
    }
}
