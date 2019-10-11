namespace TestOkur.WebApi.Infrastructure
{
    using System;
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using TestOkur.Infrastructure.CommandsQueries;

    public class CommandQueryLogger : ICommandQueryLogger
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<CommandQueryLogger> _logger;

        public CommandQueryLogger(IPublishEndpoint publishEndpoint, ILogger<CommandQueryLogger> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task LogAsync(object message)
        {
            try
            {
                await _publishEndpoint.Publish(new CommandQueryLogEvent(
                    JsonConvert.SerializeObject(message), message.GetType().ToString()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while logging command/query");
            }
        }
    }
}
