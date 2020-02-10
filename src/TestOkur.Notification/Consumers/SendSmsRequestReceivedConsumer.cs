using MongoDB.Driver;

namespace TestOkur.Notification.Consumers
{
    using MassTransit;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.Contracts.Sms;
    using TestOkur.Notification.Dtos;
    using TestOkur.Notification.Events;
    using TestOkur.Notification.Infrastructure;
    using TestOkur.Notification.Infrastructure.Clients;
    using TestOkur.Notification.Infrastructure.Data;

    public class SendSmsRequestReceivedConsumer : IConsumer<ISendSmsRequestReceived>
    {
        private const int MaxThreadCount = 50;

        private readonly ILogger<SendSmsRequestReceivedConsumer> _logger;
        private readonly ISmsClient _smsClient;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISmsRepository _smsRepository;
        private readonly ISmsLogRepository _smsLogRepository;

        public SendSmsRequestReceivedConsumer(
            IPublishEndpoint publishEndpoint,
            ISmsClient smsClient,
            ISmsRepository smsRepository,
            ILogger<SendSmsRequestReceivedConsumer> logger,
            ISmsLogRepository smsLogRepository)
        {
            _publishEndpoint = publishEndpoint;
            _smsClient = smsClient;
            _smsRepository = smsRepository;
            _logger = logger;
            _smsLogRepository = smsLogRepository;
        }

        public async Task Consume(ConsumeContext<ISendSmsRequestReceived> context)
        {
            await _smsLogRepository.LogAsync(SmsLog.CreateUsageLog(context.Message));
            var smsList = context.Message.SmsMessages.Select(s => new Sms(context.Message, s));
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

        private Task UpdateSmsStatusAsync(Exception ex, Sms message)
        {
            if (ex is SmsException smsException)
            {
                message.UserFriendlyErrorMessage = smsException.Message;
            }

            message.Error = ex.ToString();
            message.Status = SmsStatus.Failed;
            return _smsRepository.UpdateSmsAsync(message);
        }

        private async Task StoreAsync(IEnumerable<Sms> list)
        {
            const int maxTryCount = 10;
            var tryCount = 0;

            do
            {
                try
                {
                    await _smsRepository.AddManyAsync(list);
                    return;
                }
                catch (MongoBulkWriteException ex)
                {
                    _logger.LogError(ex, "Possible GUID duplication in SMS bulk insert");

                    foreach (var sms in list)
                    {
                        sms.Id = Guid.NewGuid();
                    }
                }
            } while (tryCount++ < maxTryCount);
        }

        private Task PublishSmsRequestFailedEventAsync(ConsumeContext<ISendSmsRequestReceived> context, Sms sms, Exception ex)
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

            return _publishEndpoint.Publish(@event);
        }
    }
}
