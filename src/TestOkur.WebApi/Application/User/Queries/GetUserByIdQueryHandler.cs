namespace TestOkur.WebApi.Application.User.Queries
{
    using Paramore.Darker;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class GetUserByIdQueryHandler : QueryHandlerAsync<GetUserByIdQuery, UserReadModel>
    {
        private readonly IQueryProcessor _queryProcessor;

        public GetUserByIdQueryHandler(IQueryProcessor queryProcessor)
        {
            _queryProcessor = queryProcessor;
        }

        public override async Task<UserReadModel> ExecuteAsync(
            GetUserByIdQuery query,
            CancellationToken cancellationToken = default)
        {
            var users = await _queryProcessor
                .ExecuteAsync(GetAllUsersQuery.Default, cancellationToken);

            return users.First(u => u.Id == query.UserIdToBeQueried);
        }
    }
}