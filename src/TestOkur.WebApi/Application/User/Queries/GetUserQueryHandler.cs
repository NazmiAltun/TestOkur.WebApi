namespace TestOkur.WebApi.Application.User.Queries
{
    using Paramore.Darker;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class GetUserQueryHandler : QueryHandlerAsync<GetUserQuery, UserReadModel>
    {
        private readonly IQueryProcessor _queryProcessor;

        public GetUserQueryHandler(IQueryProcessor queryProcessor)
        {
            _queryProcessor = queryProcessor;
        }

        public override async Task<UserReadModel> ExecuteAsync(
            GetUserQuery query,
            CancellationToken cancellationToken = default)
        {
            var users = await _queryProcessor
                .ExecuteAsync(GetAllUsersQuery.Default, cancellationToken);

            return string.IsNullOrEmpty(query.Email)
                ? users.First(u => u.Id == query.UserId)
                : users.First(u => u.Email == query.Email);
        }
    }
}
