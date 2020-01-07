namespace TestOkur.WebApi.Application.Lesson.Queries
{
    using System;
    using System.Collections.Generic;
    using Paramore.Darker;
    using TestOkur.Infrastructure.CommandsQueries;

    public sealed class GetSharedLessonQuery :
        IQuery<IReadOnlyCollection<LessonReadModel>>,
        ICacheResult
    {
        public GetSharedLessonQuery()
        {
        }

        public static GetSharedLessonQuery Default { get; } = new GetSharedLessonQuery();

        public string CacheKey => "SharedLessons";

        public TimeSpan CacheDuration => TimeSpan.FromDays(1);
    }
}
