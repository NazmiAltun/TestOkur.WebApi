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
	using TestOkur.Notification.Models;

	public class SendSmsRequestReceivedConsumer : IConsumer<ISendSmsRequestReceived>
	{
		private readonly ILogger<SendSmsRequestReceivedConsumer> _logger;
		private readonly ISmsClient _smsClient;
		private readonly IWebApiClient _webApiClient;
		private readonly IPublishEndpoint _publishEndpoint;
		private readonly ISmsRepository _smsRepository;

		public SendSmsRequestReceivedConsumer(
            IWebApiClient webApiClient,
            IPublishEndpoint publishEndpoint,
            ISmsClient smsClient,
            ISmsRepository smsRepository,
            ILogger<SendSmsRequestReceivedConsumer> logger)
        {
            _webApiClient = webApiClient;
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
                    var smsBody = await _smsClient.SendAsync(message);
                    if (context.Message.UserId != default)
                    {
	                    await _webApiClient.DeductSmsCreditsAsync(context.Message.UserId, smsBody);
                    }
                }
                catch (Exception ex)
                {
	                _logger.LogError(ex, "Error while sending sms");
	                await PublishSmsRequestFailedEventAsync(context, message, ex);
                }
            }
        }

		private List<Sms> ToSmsList(ConsumeContext<ISendSmsRequestReceived> context)
        {
	        return context.Message.SmsMessages
		        .Select(s => new Sms(s)
		        {
			        CreatedOnDateTimeUtc = context.Message.CreatedOnUTC,
			        UserId = context.Message.UserId,
			        UserSubjectId = context.Message.UserSubjectId,
		        }).ToList();
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
                ex.Message,
                userFriendlyMessage,
                context.Message.UserEmail);

            await _publishEndpoint.Publish(@event);
        }
    }
}
