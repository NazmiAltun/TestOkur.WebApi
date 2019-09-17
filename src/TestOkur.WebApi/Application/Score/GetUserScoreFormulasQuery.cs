namespace TestOkur.WebApi.Application.Score
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.Cqrs;

    public class GetUserScoreFormulasQuery
		: QueryBase<IReadOnlyCollection<ScoreFormulaReadModel>>,
		ICacheResult
	{
		public string CacheKey => $"ScoreFormulas_{UserId}";

		public TimeSpan CacheDuration => TimeSpan.FromHours(4);
	}
}
