using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace TestOkur.Notification.Consumers
{
    using MassTransit;
    using System.Threading.Tasks;
    using TestOkur.Contracts.Sms;
    using TestOkur.Notification.Infrastructure;

    internal class SendSmsRequestFailedConsumer : IConsumer<ISendSmsRequestFailed>
    {
        private readonly ILogger<SendSmsRequestFailedConsumer> _logger;
        private readonly INotificationFacade _notificationFacade;

        public SendSmsRequestFailedConsumer(INotificationFacade notificationFacade, ILogger<SendSmsRequestFailedConsumer> logger)
        {
            _notificationFacade = notificationFacade;
            _logger = logger;
        }

        public Task Consume(ConsumeContext<ISendSmsRequestFailed> context)
        {
            _logger.LogError($"{JsonSerializer.Serialize(context.Message)}");
            return Task.CompletedTask;
        }
    }
}
