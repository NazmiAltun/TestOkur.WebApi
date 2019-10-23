namespace TestOkur.Notification.Consumers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.Extensions.Logging;
    using TestOkur.Common;
    using TestOkur.Contracts.Sms;
    using TestOkur.Notification.Events;
    using TestOkur.Notification.Infrastructure;
    using TestOkur.Notification.Infrastructure.Clients;
    using TestOkur.Notification.Infrastructure.Data;
    using TestOkur.Notification.Models;

    public class SendSmsRequestReceivedConsumer : IConsumer<ISendSmsRequestReceived>
    {
        private readonly ILogger<SendSmsRequestReceivedConsumer> _logger;
        private readonly ISmsClient _smsClient;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISmsRepository _smsRepository;

        public SendSmsRequestReceivedConsumer(
            IPublishEndpoint publishEndpoint,
            ISmsClient smsClient,
            ISmsRepository smsRepository,
            ILogger<SendSmsRequestReceivedConsumer> logger)
        {
            _publishEndpoint = publishEndpoint;
            _smsClient = smsClient;
            _smsRepository = smsRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ISendSmsRequestReceived> context)
        {
            var smsList = ToSmsList(context);
            await StoreAsync(smsList);

            foreach (var message in smsList)
            {
                try
                {
                    await _smsClient.SendAsync(message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while sending sms");
                    await UpdateSmsStatusAsync(ex, message);
                    await PublishSmsRequestFailedEventAsync(context, message, ex);
                }
            }
        }

        private async Task UpdateSmsStatusAsync(Exception ex, Sms message)
        {
            if (ex is SmsException smsException)
            {
                message.UserFriendlyErrorMessage = smsException.Message;
            }

            message.Error = ex.ToString();
            message.Status = SmsStatus.Failed;
            await _smsRepository.UpdateSmsAsync(message);
        }

        private List<Sms> ToSmsList(ConsumeContext<ISendSmsRequestReceived> context)
        {
            return context.Message.SmsMessages
                .Select(s => new Sms(context.Message, s)).ToList();
        }

        private async Task StoreAsync(IEnumerable<Sms> list)
        {
            await _smsRepository.AddManyAsync(list);
        }

        private async Task PublishSmsRequestFailedEventAsync(ConsumeContext<ISendSmsRequestReceived> context, Sms sms, Exception ex)
        {
            var userFriendlyMessage = ErrorCodes.SmsSystemFailure;

            if (ex is SmsException exception)
            {
                userFriendlyMessage = exception.Message;
            }

            var @event = new SendSmsRequestFailed(
                context.Message.UserId,
                sms.Phone,
                sms.Body,
                ex.ToString(),
                userFriendlyMessage,
                context.Message.UserEmail);

            await _publishEndpoint.Publish(@event);
        }
    }
}
