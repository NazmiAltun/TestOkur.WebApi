namespace TestOkur.WebApi.Application.OpticalForm
{
    using System;
    using System.Collections.Generic;
    using Paramore.Darker;
    using TestOkur.Infrastructure.Cqrs;

    public sealed class GetAllOpticalFormTypesQuery :
        IQuery<IReadOnlyCollection<OpticalFormTypeReadModel>>,
        ICacheResult
    {
        public string CacheKey => "OpticalFormTypes";

        public TimeSpan CacheDuration => TimeSpan.FromDays(1);
    }
}
