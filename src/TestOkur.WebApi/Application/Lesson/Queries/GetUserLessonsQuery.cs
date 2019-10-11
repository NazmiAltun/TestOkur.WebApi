namespace TestOkur.WebApi.Application.Lesson.Queries
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.CommandsQueries;

    public class GetUserLessonsQuery : QueryBase<IReadOnlyCollection<LessonReadModel>>,
        ICacheResult
    {
        public GetUserLessonsQuery(int userId)
            : base(userId)
        {
        }

        public GetUserLessonsQuery()
        {
        }

        public string CacheKey => $"Lessons_{UserId}";

        public TimeSpan CacheDuration => TimeSpan.FromHours(4);
    }
}
