namespace TestOkur.WebApi.Application.Exam.Queries
{
    using System;
    using System.Collections.Generic;
    using Paramore.Darker;
    using TestOkur.Infrastructure.CommandsQueries;

    public sealed class ExamTypeQuery : IQuery<IReadOnlyCollection<ExamTypeReadModel>>, ICacheResult
    {
        private ExamTypeQuery()
        {
        }

        public static ExamTypeQuery Default { get; } = new ExamTypeQuery();

        public string CacheKey => "ExamTypes";

        public TimeSpan CacheDuration => TimeSpan.FromDays(1);
    }
}
