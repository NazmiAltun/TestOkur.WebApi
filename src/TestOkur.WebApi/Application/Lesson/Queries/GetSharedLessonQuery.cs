namespace TestOkur.WebApi.Application.Lesson.Queries
{
    using System;
    using System.Collections.Generic;
    using Paramore.Darker;
    using TestOkur.Infrastructure.Cqrs;

    public sealed class GetSharedLessonQuery :
        IQuery<IReadOnlyCollection<LessonReadModel>>,
        ICacheResult
    {
        public string CacheKey => "SharedLessons";

        public TimeSpan CacheDuration => TimeSpan.FromDays(1);
    }
}
