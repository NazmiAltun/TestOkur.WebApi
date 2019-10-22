namespace TestOkur.WebApi.Application.User.Queries
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Paramore.Darker;

    public sealed class GetUserQueryHandler : QueryHandlerAsync<GetUserQuery, UserReadModel>
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly ILogger<GetUserQueryHandler> _logger;

        public GetUserQueryHandler(IQueryProcessor queryProcessor, ILogger<GetUserQueryHandler> logger)
        {
            _queryProcessor = queryProcessor;
            _logger = logger;
        }

        public override async Task<UserReadModel> ExecuteAsync(
            GetUserQuery query,
            CancellationToken cancellationToken = default)
        {
            var users = await _queryProcessor.ExecuteAsync(new GetAllUsersQuery(), cancellationToken);

            _logger.LogDebug($"Email {query.Email} UserId {query.UserId}");
            return string.IsNullOrEmpty(query.Email)
                ? users.First(u => u.Id == query.UserId)
                : users.First(u => u.Email == query.Email);
        }
    }
}
