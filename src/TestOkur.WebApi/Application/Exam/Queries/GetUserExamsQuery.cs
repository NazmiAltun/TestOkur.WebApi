namespace TestOkur.WebApi.Application.Exam.Queries
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.Cqrs;

    public class GetUserExamsQuery :
        QueryBase<IReadOnlyCollection<ExamReadModel>>,
        ICacheResult
    {
        public string CacheKey => $"Exams_{UserId}";

        public TimeSpan CacheDuration => TimeSpan.FromHours(4);
    }
}
