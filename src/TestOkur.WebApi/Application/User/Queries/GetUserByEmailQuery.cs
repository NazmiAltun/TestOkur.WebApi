namespace TestOkur.WebApi.Application.User.Queries
{
    using TestOkur.Infrastructure.Cqrs;

    public class GetUserByEmailQuery : QueryBase<UserReadModel>
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
