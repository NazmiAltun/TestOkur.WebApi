namespace TestOkur.WebApi.Application.Lesson.Queries
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.Cqrs;

    public class GetUserLessonsQuery : QueryBase<IReadOnlyCollection<LessonReadModel>>,
        ICacheResult
    {
        public string CacheKey => $"Lessons_{UserId}";

        public TimeSpan CacheDuration => TimeSpan.FromHours(4);
    }
}
