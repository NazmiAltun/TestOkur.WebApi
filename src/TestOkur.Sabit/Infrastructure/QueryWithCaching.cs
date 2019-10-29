namespace TestOkur.Sabit.Infrastructure
{
    using System;
    using Paramore.Darker;
    using TestOkur.Infrastructure.CommandsQueries;

    public abstract class QueryWithCaching<T> : IQuery<T>, ICacheResult
    {
        public string CacheKey => GetType().Name;

        public TimeSpan CacheDuration => TimeSpan.FromDays(300);
    }
}
