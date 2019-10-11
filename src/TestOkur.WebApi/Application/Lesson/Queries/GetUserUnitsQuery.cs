namespace TestOkur.WebApi.Application.Lesson.Queries
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.CommandsQueries;

    public class GetUserUnitsQuery : QueryBase<IReadOnlyCollection<UnitReadModel>>,
        ICacheResult
    {
        public GetUserUnitsQuery()
        {
        }

        public GetUserUnitsQuery(int userId)
         : base(userId)
        {
        }

        public string CacheKey => $"Units_{UserId}";

        public TimeSpan CacheDuration => TimeSpan.FromHours(2);
    }
}
