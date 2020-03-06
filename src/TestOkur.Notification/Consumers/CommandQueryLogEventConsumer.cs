namespace TestOkur.Notification.Consumers
{
    using MassTransit;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;
    using TestOkur.Contracts.App;
    using TestOkur.Notification.Models;
    using TestOkur.Serialization;

    public class CommandQueryLogEventConsumer : IConsumer<ICommandQueryLogEvent>
    {
        private readonly ILogger<CommandQueryLogEventConsumer> _logger;

        public CommandQueryLogEventConsumer(ILogger<CommandQueryLogEventConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<ICommandQueryLogEvent> context)
        {
            _logger.LogInformation(JsonUtils.Serialize(new CommandQueryLog(context.Message)));

            return Task.CompletedTask;
        }
    }
}
