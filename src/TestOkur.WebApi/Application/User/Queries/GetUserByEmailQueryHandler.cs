namespace TestOkur.WebApi.Application.User.Queries
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Paramore.Darker;
    using Paramore.Darker.QueryLogging;
    using TestOkur.Infrastructure.CommandsQueries;

    public sealed class GetUserByEmailQueryHandler : QueryHandlerAsync<GetUserByEmailQuery, UserReadModel>
    {
        private readonly IProcessor _processor;

        public GetUserByEmailQueryHandler(IProcessor processor)
        {
            _processor = processor;
        }

        [QueryLogging(2)]
        public override async Task<UserReadModel> ExecuteAsync(
            GetUserByEmailQuery query,
            CancellationToken cancellationToken = default)
        {
            var users = await _processor.ExecuteAsync<GetAllUsersQuery, IReadOnlyCollection<UserReadModel>>(
                new GetAllUsersQuery(),
                cancellationToken);

            return string.IsNullOrEmpty(query.Email)
                ? users.First(u => u.Id == query.UserId)
                : users.First(u => u.Email == query.Email);
        }
    }
}
