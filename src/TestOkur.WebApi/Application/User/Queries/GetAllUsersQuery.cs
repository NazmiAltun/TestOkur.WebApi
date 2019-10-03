namespace TestOkur.WebApi.Application.User.Queries
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.Cqrs;

    public class GetAllUsersQuery :
        QueryBase<IReadOnlyCollection<UserReadModel>>,
        ICacheResult,
        ISkipLogging
    {
        public string CacheKey => "Users";

        public TimeSpan CacheDuration => TimeSpan.FromHours(4);
    }
}
