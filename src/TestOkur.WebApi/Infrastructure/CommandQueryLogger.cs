namespace TestOkur.WebApi.Infrastructure
{
    using Microsoft.Extensions.Logging;
    using Paramore.Brighter;
    using Paramore.Darker;
    using System;
    using System.Threading.Tasks;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.Serialization;

    public class CommandQueryLogger : ICommandQueryLogger
    {
        private readonly ILogger<CommandQueryLogger> _logger;
        private readonly IUserIdProvider _userIdProvider;

        public CommandQueryLogger(ILogger<CommandQueryLogger> logger, IUserIdProvider userIdProvider)
        {
            _logger = logger;
            _userIdProvider = userIdProvider;
        }

        public Task LogQueryAsync<TQuery>(TQuery query)
            where TQuery : IQuery
        {
            //try
            //{
            //    var type = query.GetType().ToString();
            //    var queryContent = JsonUtils.Serialize(query);
            //    _logger.LogWarning("Query. Type : {Type} - {QueryContent}", type, queryContent);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "Error occured while logging query");
            //}

            return Task.CompletedTask;
        }

        public void LogQuery<TQuery>(TQuery query)
            where TQuery : IQuery
        {
            //try
            //{
            //    var type = query.GetType().ToString();
            //    var queryContent = JsonUtils.Serialize(query);
            //    _logger.LogWarning("Query. Type : {Type} - {QueryContent}", type, queryContent);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "Error occured while logging query");
            //}
        }

        public async Task LogCommandAsync<TCommand>(TCommand command)
            where TCommand : IRequest
        {
            try
            {
                var type = command.GetType().ToString();
                var commandContent = JsonUtils.Serialize(command);
                var userId = await _userIdProvider.GetAsync();
                _logger.LogWarning(
                    "Command. UserId : {userId}. Type : {Type} - {CommandContent}", userId, type, commandContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while logging query");
            }
        }

        public void LogCommand<TCommand>(TCommand command)
            where TCommand : IRequest
        {
            try
            {
                var type = command.GetType().ToString();
                var commandContent = JsonUtils.Serialize(command);
                var userId = _userIdProvider.Get();

                _logger.LogWarning(
                    "Command. UserId : {userId}. Type : {Type} - {CommandContent}", userId, type, commandContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while logging query");
            }
        }
    }
}
