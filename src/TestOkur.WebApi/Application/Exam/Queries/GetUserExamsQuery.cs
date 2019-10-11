namespace TestOkur.WebApi.Application.Exam.Queries
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.CommandsQueries;

    public class GetUserExamsQuery :
        QueryBase<IReadOnlyCollection<ExamReadModel>>,
        ICacheResultWithRegion
    {
        public GetUserExamsQuery()
        {
        }

        public GetUserExamsQuery(int userId)
            : base(userId)
        {
        }

        public string Region => "Exams";

        public string CacheKey => $"Exams_{UserId}";

        public TimeSpan CacheDuration => TimeSpan.FromHours(4);
    }
}
