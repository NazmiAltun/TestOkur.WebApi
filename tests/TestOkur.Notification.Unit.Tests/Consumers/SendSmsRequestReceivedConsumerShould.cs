namespace TestOkur.Notification.Unit.Tests.Consumers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MassTransit;
    using Microsoft.Extensions.Logging;
    using NSubstitute;
    using RichardSzalay.MockHttp;
    using TestOkur.Contracts.Sms;
    using TestOkur.Notification.Configuration;
    using TestOkur.Notification.Consumers;
    using TestOkur.Notification.Infrastructure.Clients;
    using TestOkur.Notification.Infrastructure.Data;
    using Xunit;

    public class SendSmsRequestReceivedConsumerShould
    {
        private readonly IWebApiClient _webApiClient;
        private readonly SmsConfiguration _configuration;

        public SendSmsRequestReceivedConsumerShould()
        {
            _configuration = new SmsConfiguration
            {
                Password = "Test",
                ServiceUrl = "http://localhost/send-sms",
                User = "Test",
            };
            _webApiClient = Substitute.For<IWebApiClient>();
        }

        [Fact]
        public async Task DeductSmsCreditsFromAccountsCreditBalance_When_SmsSuccessfullySent()
        {
            const int userId = 15;
            const string messageBody = "Hello There!";
            var smsDeducted = false;

            _webApiClient.DeductSmsCreditsAsync(userId, messageBody)
                .Returns(Task.FromResult((object)null))
                .AndDoes(x => smsDeducted = true);

            var publishEndPoint = Substitute.For<IPublishEndpoint>();
            var smsRepository = Substitute.For<ISmsRepository>();
            var logger = Substitute.For<ILogger<SendSmsRequestReceivedConsumer>>();
            var smsMessages = new List<ISmsMessage>
                {
                    new SmsMessage("TEST", messageBody, "42342"),
                };

            var consumerContext = Substitute.For<ConsumeContext<ISendSmsRequestReceived>>();
            consumerContext.Message.UserId.Returns(userId);
            consumerContext.Message.SmsMessages.Returns(smsMessages);

            var consumer = new SendSmsRequestReceivedConsumer(
                _webApiClient,
                publishEndPoint,
                CreateSmsClient("text/plain", "123123"),
                smsRepository,
                logger);

            await consumer.Consume(consumerContext);
            smsDeducted.Should().BeTrue();
        }

        [Fact]
        public async Task PublishSmsRequestFailedEvent_When_SmsCannotBeSent()
        {
            var eventList = new List<ISendSmsRequestFailed>();
            var publishEndPoint = Substitute.For<IPublishEndpoint>();
            publishEndPoint.Publish(Arg.Any<ISendSmsRequestFailed>())
                .Returns(Task.FromResult((object)null))
                .AndDoes(x => eventList.Add(x.Arg<ISendSmsRequestFailed>()));
            var smsRepository = Substitute.For<ISmsRepository>();
            var logger = Substitute.For<ILogger<SendSmsRequestReceivedConsumer>>();
            var smsMessages = new List<ISmsMessage>
                {
                    new SmsMessage("TEST", "TEST", "42342"),
                    new SmsMessage("TEST", "TEST", "42342"),
                    new SmsMessage("TEST", "TEST", "42342"),
                };
            var consumerContext = Substitute.For<ConsumeContext<ISendSmsRequestReceived>>();
            consumerContext.Message.SmsMessages.Returns(smsMessages);

            var consumer = new SendSmsRequestReceivedConsumer(
                _webApiClient,
                publishEndPoint,
                CreateSmsClient("text/plain", "HATA|310|Uzun mesaj metni"),
                smsRepository,
                logger);

            await consumer.Consume(consumerContext);
            eventList.First().UserFriendlyMessage.Should().Contain("Uzun mesaj metni");
            eventList.Should().HaveCount(smsMessages.Count);
        }

        private SmsClient CreateSmsClient(string mediaType, string content)
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(HttpMethod.Post, "http://localhost/send-sms")
                .Respond(mediaType, content);

            return new SmsClient(mockHttp.ToHttpClient(), _configuration);
        }

        private class SmsMessage : ISmsMessage
        {
            public SmsMessage(
                string subject,
                string body,
                string receiver)
            {
                Subject = subject;
                Body = body;
                Receiver = receiver;
                Id = Guid.NewGuid();
                Credit = 1;
            }

            public Guid Id { get; }

            public int Credit { get; }

            public string StudentOpticalFormId { get; }

            public int ExamId { get; }

            public string Subject { get; }

            public string Body { get; }

            public string Receiver { get; }
        }
    }
}
