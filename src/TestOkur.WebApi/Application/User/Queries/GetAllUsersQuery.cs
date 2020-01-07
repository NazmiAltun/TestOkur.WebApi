namespace TestOkur.WebApi.Application.User.Queries
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.CommandsQueries;

    public class GetAllUsersQuery :
        QueryBase<IReadOnlyCollection<UserReadModel>>,
        ICacheResult,
        ISkipLogging
    {
        private GetAllUsersQuery()
        {
        }

        public static GetAllUsersQuery Default { get; } = new GetAllUsersQuery();

        public string CacheKey => "Users";

        public TimeSpan CacheDuration => TimeSpan.FromHours(4);
    }
}
