namespace TestOkur.Sabit.Infrastructure
{
    using System.Threading.Tasks;
    using TestOkur.Infrastructure.CommandsQueries;

    public class StubCommandQueryLogger : ICommandQueryLogger
    {
        public Task LogAsync(object message)
        {
            throw new System.NotImplementedException();
        }

        public void Log(object message)
        {
            throw new System.NotImplementedException();
        }
    }
}