namespace TestOkur.WebApi.Application.User.Queries
{
    using TestOkur.Infrastructure.CommandsQueries;

    public class GetUserByEmailQuery : QueryBase<UserReadModel>, ISkipLogging
    {
        public GetUserByEmailQuery()
        {
        }

        public GetUserByEmailQuery(string email)
        {
            Email = email;
        }

        public string Email { get; }
    }
}
