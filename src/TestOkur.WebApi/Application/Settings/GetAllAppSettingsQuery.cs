namespace TestOkur.WebApi.Application.Settings
{
	using System;
	using System.Collections.Generic;
	using Paramore.Darker;
	using TestOkur.Infrastructure.Cqrs;

	public sealed class GetAllAppSettingsQuery :
        IQuery<IReadOnlyCollection<AppSettingReadModel>>,
        ICacheResult
    {
        public string CacheKey => "AppSettings";

        public TimeSpan CacheDuration => TimeSpan.FromDays(14);
    }
}
