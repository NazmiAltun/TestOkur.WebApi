namespace TestOkur.WebApi.Application.User.Queries
{
    using TestOkur.Infrastructure.CommandsQueries;

    public class GetUserByIdQuery : QueryBase<UserReadModel>, ISkipLogging
    {
        public GetUserByIdQuery(int userIdToBeQueried)
        {
            UserIdToBeQueried = userIdToBeQueried;
        }

        public int UserIdToBeQueried { get; }
    }
}