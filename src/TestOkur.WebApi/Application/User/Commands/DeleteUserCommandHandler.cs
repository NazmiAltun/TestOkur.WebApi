namespace TestOkur.WebApi.Application.User.Commands
{
    using Dapper;
    using Npgsql;
    using Paramore.Brighter;
    using Paramore.Darker;
    using System.Threading;
    using System.Threading.Tasks;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.WebApi.Application.User.Clients;
    using TestOkur.WebApi.Application.User.Queries;
    using TestOkur.WebApi.Configuration;

    public class DeleteUserCommandHandler : RequestHandlerAsync<DeleteUserCommand>
    {
        private const string sql = @"DELETE FROM users WHERE id=@id";

        private readonly IQueryProcessor _queryProcessor;
        private readonly IIdentityClient _identityClient;
        private readonly string _connectionString;

        public DeleteUserCommandHandler(
            ApplicationConfiguration configurationOptions,
            IIdentityClient identityClient,
            IQueryProcessor queryProcessor)
        {
            _connectionString = configurationOptions.Postgres;
            _identityClient = identityClient;
            _queryProcessor = queryProcessor;
        }

        [Idempotent(1)]
        [ClearCache(2)]
        public override async Task<DeleteUserCommand> HandleAsync(
            DeleteUserCommand command,
            CancellationToken cancellationToken = default)
        {
            await Task.WhenAll(
                DeleteUserRecordAsync(command),
                DeleteUserIdentityAsync(command, cancellationToken));

            return await base.HandleAsync(command, cancellationToken);
        }

        private async Task DeleteUserRecordAsync(DeleteUserCommand command)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(sql, new { id = command.DeleteUserId });
        }

        private async Task DeleteUserIdentityAsync(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            var user = await FindUserAsync(command.DeleteUserId);
            await _identityClient.DeleteUserAsync(user.SubjectId, cancellationToken);
        }

        private Task<UserReadModel> FindUserAsync(int userId)
        {
            return _queryProcessor.ExecuteAsync(new GetUserByIdQuery(userId));
        }
    }
}
