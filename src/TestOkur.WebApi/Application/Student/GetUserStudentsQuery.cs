namespace TestOkur.WebApi.Application.Student
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.CommandsQueries;

    public sealed class GetUserStudentsQuery :
        QueryBase<IReadOnlyCollection<StudentReadModel>>,
        ICacheResult
    {
        public GetUserStudentsQuery(int userId)
            : base(userId)
        {
        }

        public GetUserStudentsQuery()
        {
        }

        public string CacheKey => $"Students_{UserId}";

        public TimeSpan CacheDuration => TimeSpan.FromHours(2);
    }
}
