namespace TestOkur.WebApi.Application.LicenseType
{
	using System;
	using System.Collections.Generic;
	using Paramore.Darker;
	using TestOkur.Infrastructure.Cqrs;

	public sealed class GetAllLicenseTypesQuery :
        IQuery<IReadOnlyCollection<LicenseTypeReadModel>>,
        ICacheResult
    {
        public string CacheKey => "LicenseTypes";

        public TimeSpan CacheDuration => TimeSpan.FromDays(1);
    }
}
