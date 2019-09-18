namespace TestOkur.WebApi.Application.Exam.Queries
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.Cqrs;

    public class GetUserExamsQuery :
        QueryBase<IReadOnlyCollection<ExamReadModel>>,
        ICacheResult
    {
        public GetUserExamsQuery()
        {
        }

        public GetUserExamsQuery(int userId)
            : base(userId)
        {
        }

        public string CacheKey => $"Exams_{UserId}";

        public TimeSpan CacheDuration => TimeSpan.FromHours(4);
    }
}
