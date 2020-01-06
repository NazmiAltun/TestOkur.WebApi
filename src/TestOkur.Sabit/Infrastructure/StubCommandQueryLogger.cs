namespace TestOkur.Sabit.Infrastructure
{
    using Paramore.Brighter;
    using Paramore.Darker;
    using System.Threading.Tasks;
    using TestOkur.Infrastructure.CommandsQueries;

    public class StubCommandQueryLogger : ICommandQueryLogger
    {
        public Task LogQueryAsync<TQuery>(TQuery query)
            where TQuery : IQuery
        {
            throw new System.NotImplementedException();
        }

        public void LogQuery<TQuery>(TQuery query)
            where TQuery : IQuery
        {
            throw new System.NotImplementedException();
        }

        public Task LogCommandAsync<TCommand>(TCommand query)
            where TCommand : IRequest
        {
            throw new System.NotImplementedException();
        }

        public void LogCommand<TCommand>(TCommand query)
            where TCommand : IRequest
        {
            throw new System.NotImplementedException();
        }
    }
}