namespace TestOkur.WebApi.Infrastructure
{
    using MassTransit;
    using Microsoft.Extensions.Logging;
    using Paramore.Brighter;
    using Paramore.Darker;
    using System;
    using System.Threading.Tasks;
    using TestOkur.Common.Helpers;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.Serializer;

    public class CommandQueryLogger : ICommandQueryLogger
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<CommandQueryLogger> _logger;

        public CommandQueryLogger(IPublishEndpoint publishEndpoint, ILogger<CommandQueryLogger> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task LogQueryAsync<TQuery>(TQuery query)
            where TQuery : IQuery
        {
            try
            {
                await _publishEndpoint.Publish(new CommandQueryLogEvent(
                    JsonUtils.Serialize(query), query.GetType().ToString()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while logging query");
            }
        }

        public void LogQuery<TQuery>(TQuery query)
            where TQuery : IQuery
        {
            try
            {
                AsyncHelper.RunSync(() =>
                    _publishEndpoint.Publish(new CommandQueryLogEvent(
                    JsonUtils.Serialize(query), query.GetType().ToString())));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while logging query");
            }
        }

        public async Task LogCommandAsync<TCommand>(TCommand command)
            where TCommand : IRequest
        {
            try
            {
                await _publishEndpoint.Publish(new CommandQueryLogEvent(
                    JsonUtils.Serialize(command), command.GetType().ToString()));
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
                AsyncHelper.RunSync(() =>
                    _publishEndpoint.Publish(new CommandQueryLogEvent(
                        JsonUtils.Serialize(command), command.GetType().ToString())));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while logging query");
            }
        }
    }
}
