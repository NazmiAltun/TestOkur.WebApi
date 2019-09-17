namespace TestOkur.WebApi.Application.Student
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.Cqrs;

    public sealed class GetUserStudentsQuery :
        QueryBase<IReadOnlyCollection<StudentReadModel>>,
        ICacheResult
    {
        public string CacheKey => $"Students_{UserId}";

        public TimeSpan CacheDuration => TimeSpan.FromHours(2);
    }
}
