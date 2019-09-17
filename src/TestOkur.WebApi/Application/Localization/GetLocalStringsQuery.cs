namespace TestOkur.WebApi.Application.Localization
{
    using System;
    using System.Collections.Generic;
    using Paramore.Darker;
    using TestOkur.Infrastructure.Cqrs;

    public sealed class GetLocalStringsQuery :
		IQuery<IReadOnlyCollection<LocalString>>,
		ICacheResult
	{
		public GetLocalStringsQuery(string cultureCode)
		{
			CultureCode = cultureCode;
		}

		public string CultureCode { get; }

		public string CacheKey => CultureCode;

		public TimeSpan CacheDuration => TimeSpan.FromDays(1);
	}
}
