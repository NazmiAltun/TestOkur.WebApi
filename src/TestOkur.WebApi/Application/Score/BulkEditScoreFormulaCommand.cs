namespace TestOkur.WebApi.Application.Score
{
    using System.Collections.Generic;
    using TestOkur.Infrastructure.CommandsQueries;

    public class BulkEditScoreFormulaCommand : CommandBase, IClearCache
    {
        public BulkEditScoreFormulaCommand(IEnumerable<EditScoreFormulaCommand> commands)
        {
            Commands = commands;
        }

        public BulkEditScoreFormulaCommand()
        {
        }

        public IEnumerable<string> CacheKeys => new[] { $"ScoreFormulas_{UserId}" };

        public IEnumerable<EditScoreFormulaCommand> Commands { get; set; }
    }
}
