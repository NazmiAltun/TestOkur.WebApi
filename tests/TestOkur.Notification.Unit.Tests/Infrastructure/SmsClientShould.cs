namespace TestOkur.Notification.Unit.Tests.Infrastructure
{
	using System;
	using System.Net;
	using System.Net.Http;
	using System.Threading.Tasks;
	using FluentAssertions;
	using RichardSzalay.MockHttp;
	using TestOkur.Notification.Configuration;
	using TestOkur.Notification.Infrastructure;
	using TestOkur.Notification.Models;
	using Xunit;

	public class SmsClientShould
	{
		private readonly SmsConfiguration _configuration;

		public SmsClientShould()
		{
			_configuration = new SmsConfiguration
			{
				Password = "Test",
				ServiceUrl = "http://localhost/send-sms",
				User = "Test",
			};
		}

		[Fact]
		public async Task SendSms_When_ValidModelPosted()
		{
			var smsClient = CreateSmsClient("text/plain", "SUCCESS");

			Func<Task> act = async () =>
			{
				await SendAsync(smsClient, "TESt", "TEST", "54353");
			};
			await act.Should().NotThrowAsync();
			(await SendAsync(smsClient, "TESt", "TEST", "54353"))
				.Should().Be("TEST");
		}

		[Fact]
		public async Task Throw_When_Endpoint_NotFound()
		{
			var smsClient = CreateSmsClient(HttpStatusCode.NotFound);
			Func<Task> act = async () =>
			{
				await SendAsync(smsClient, "TESt", "TEST", "54353");
			};
			await act.Should().ThrowAsync<Exception>();
		}

		[Fact]
		public async Task Throw_SmsException_When_Server_Returns_CustomErrorMessage()
		{
			var smsClient = CreateSmsClient("text/plain", "HATA|310|Uzun mesaj metni");

			Func<Task> act = async () =>
			{
				await SendAsync(smsClient, "TESt", "TEST", "54353");
			};
			(await act.Should().ThrowAsync<SmsException>())
				.Where(e => e.Message == "Uzun mesaj metni");
		}

		private async Task<string> SendAsync(SmsClient smsClient, string subject, string body, string phone)
		{
			var sms = new Sms()
			{
				Id = Guid.NewGuid(),
				Subject = subject,
				Body = body,
				Phone = phone,
			};

			return await smsClient.SendAsync(sms);
		}

		private SmsClient CreateSmsClient(HttpStatusCode httpStatusCode)
		{
			var mockHttp = new MockHttpMessageHandler();
			mockHttp.When(HttpMethod.Post, "http://localhost/send-sms")
				.Respond(httpStatusCode);

			return new SmsClient(mockHttp.ToHttpClient(), _configuration);
		}

		private SmsClient CreateSmsClient(string mediaType, string content)
		{
			var mockHttp = new MockHttpMessageHandler();
			mockHttp.When(HttpMethod.Post, "http://localhost/send-sms")
				.Respond(mediaType, content);

			return new SmsClient(mockHttp.ToHttpClient(), _configuration);
		}
	}
}
