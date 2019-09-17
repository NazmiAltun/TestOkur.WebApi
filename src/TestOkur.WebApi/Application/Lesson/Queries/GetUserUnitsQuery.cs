namespace TestOkur.WebApi.Application.Lesson.Queries
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.Cqrs;

    public class GetUserUnitsQuery : QueryBase<IReadOnlyCollection<UnitReadModel>>,
        ICacheResult
    {
        public string CacheKey => $"Units_{UserId}";

        public TimeSpan CacheDuration => TimeSpan.FromHours(2);
    }
}
