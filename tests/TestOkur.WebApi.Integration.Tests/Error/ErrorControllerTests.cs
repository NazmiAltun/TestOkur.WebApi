namespace TestOkur.WebApi.Integration.Tests.Error
{
	using System;
	using System.IO;
	using System.Net.Http;
	using System.Threading.Tasks;
	using FluentAssertions;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Http.Internal;
	using TestOkur.Contracts.Alert;
	using TestOkur.Infrastructure.Extensions;
	using TestOkur.TestHelper;
	using TestOkur.TestHelper.Extensions;
	using TestOkur.WebApi.Application.Error;
	using TestOkur.WebApi.Integration.Tests.Common;
	using Xunit;

	public class ErrorControllerTests : Test
	{
		private const string ApiPath = "api/v1/error";

		[Fact]
		public async Task When_ErrorPosted_Then_EventShouldBePublished()
		{
			using (var testServer = await CreateWithUserAsync())
			{
				var client = testServer.CreateClient();
				var imagePath = string.Empty;

				using (var stream = File.OpenRead(Path.Combine("Error", "ErrorSS.png")))
				{
					var response = await client.PostAsync($"{ApiPath}/upload", new MultipartFormDataContent()
					{
						{ new ByteArrayContent(stream.ToByteArray()), "file", "ss.png" },
					});
					response.EnsureSuccessStatusCode();
					imagePath = await response.ReadAsync<string>();
				}

				var model = new ErrorModel(
					$"{RandomGen.String(20)}@gmail.com",
					RandomGen.Next(),
					RandomGen.Next(),
					RandomGen.String(20),
					imagePath,
					"Houston!We've a problem");
				await client.PostAsync(ApiPath, model.ToJsonContent());
				Consumer.Instance.GetAll<IUserErrorReceived>()
					.Should().Contain(x =>
						x.ReporterUserId == model.ReporterUserId &&
						x.Description == model.Description &&
						x.ExamId == model.ExamId &&
						x.ImageFilePath == imagePath &&
						x.ExamName == model.ExamName);
			}
		}

		private FormFile GetFormFile()
		{
			using (var stream = File.OpenRead(@"Error\ErrorSS.png"))
			{
				return new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
				{
					Headers = new HeaderDictionary(),
					ContentType = "application/png",
				};
			}
		}
	}
}
