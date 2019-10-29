namespace TestOkur.Sabit.Infrastructure
{
    using System.Threading.Tasks;
    using TestOkur.Infrastructure.CommandsQueries;

    public class StubUserIdProvider : IUserIdProvider
    {
        public Task<int> GetAsync()
        {
            throw new System.NotImplementedException();
        }

        public int Get()
        {
            throw new System.NotImplementedException();
        }
    }
}
