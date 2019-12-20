namespace TestOkur.Notification.Consumers
{
    using MassTransit;
    using Microsoft.Extensions.Logging;
    using System.Text.Json;
    using System.Threading.Tasks;
    using TestOkur.Contracts.Sms;

    internal class SendSmsRequestFailedConsumer : IConsumer<ISendSmsRequestFailed>
    {
        private readonly ILogger<SendSmsRequestFailedConsumer> _logger;

        public SendSmsRequestFailedConsumer(ILogger<SendSmsRequestFailedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<ISendSmsRequestFailed> context)
        {
            _logger.LogError($"{JsonSerializer.Serialize(context.Message)}");
            return Task.CompletedTask;
        }
    }
}
