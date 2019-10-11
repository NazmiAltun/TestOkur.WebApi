namespace TestOkur.WebApi.Application.Score
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TestOkur.Infrastructure.CommandsQueries;

    [DataContract]
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

        [DataMember]
        public IEnumerable<EditScoreFormulaCommand> Commands { get; private set; }
    }
}
