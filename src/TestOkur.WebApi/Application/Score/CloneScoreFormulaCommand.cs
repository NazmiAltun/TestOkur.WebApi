namespace TestOkur.WebApi.Application.Score
{
    using System.Collections.Generic;
    using TestOkur.Infrastructure.Cqrs;

    public sealed class CloneScoreFormulaCommand
        : CommandBase, IClearCache
    {
        public IEnumerable<string> CacheKeys => new[] { $"ScoreFormulas_{UserId}" };
    }
}
