namespace TestOkur.WebApi.Application.City
{
    using System;
    using System.Collections.Generic;
    using Paramore.Darker;
    using TestOkur.Infrastructure.CommandsQueries;

    public sealed class GetAllCitiesQuery :
        IQuery<IReadOnlyCollection<CityReadModel>>,
        ICacheResult
    {
        public string CacheKey => "Cities";

        public TimeSpan CacheDuration => TimeSpan.FromDays(1);
    }
}
