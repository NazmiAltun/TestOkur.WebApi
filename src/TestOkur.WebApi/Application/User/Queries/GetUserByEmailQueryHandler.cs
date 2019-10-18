namespace TestOkur.WebApi.Application.User.Queries
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Paramore.Darker;

    public sealed class GetUserByEmailQueryHandler : QueryHandlerAsync<GetUserByEmailQuery, UserReadModel>
    {
        private readonly IQueryProcessor _queryProcessor;

        public GetUserByEmailQueryHandler(IQueryProcessor queryProcessor)
        {
            _queryProcessor = queryProcessor;
        }

        public override async Task<UserReadModel> ExecuteAsync(
            GetUserByEmailQuery query,
            CancellationToken cancellationToken = default)
        {
            var users = await _queryProcessor.ExecuteAsync(new GetAllUsersQuery(), cancellationToken);

            return string.IsNullOrEmpty(query.Email)
                ? users.First(u => u.Id == query.UserId)
                : users.First(u => u.Email == query.Email);
        }
    }
}
