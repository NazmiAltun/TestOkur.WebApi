namespace TestOkur.WebApi.Application.User.Queries
{
    using TestOkur.Infrastructure.CommandsQueries;

    public class GetUserQuery : QueryBase<UserReadModel>, ISkipLogging
    {
        public GetUserQuery()
        {
        }

        public GetUserQuery(string email)
        {
            Email = email;
        }

        public string Email { get; }
    }
}
