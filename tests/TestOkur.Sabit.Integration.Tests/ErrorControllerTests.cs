namespace TestOkur.Sabit.Integration.Tests
{
    using FluentAssertions;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;
    using TestOkur.Contracts.Alert;
    using TestOkur.Infrastructure.Mvc.Extensions;
    using TestOkur.Sabit.Application.Error;
    using TestOkur.Serialization;
    using TestOkur.TestHelper;
    using Xunit;

    public class ErrorControllerTests : IClassFixture<WebApplicationFactory>
    {
        private const string ApiPath = "api/v1/error";

        private readonly WebApplicationFactory _factory;

        public ErrorControllerTests(WebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task When_ErrorPosted_Then_EventShouldBePublished()
        {
            var client = _factory.CreateClient();
            var imagePath = string.Empty;

            await using var stream = File.OpenRead("ss.png");
            var response = await client.PostAsync($"{ApiPath}/upload", new MultipartFormDataContent()
            {
                {
                    new ByteArrayContent(stream.ToByteArray()), "file", "ss.png"
                },
            });
            response.EnsureSuccessStatusCode();
            imagePath = await response.Content.ReadAsStringAsync();
            var model = new ErrorModel(
                $"{RandomGen.String(20)}@gmail.com",
                RandomGen.Next().ToString(),
                RandomGen.Next(),
                RandomGen.Next(),
                RandomGen.String(20),
                imagePath,
                null,
                null,
                "Houston!We've a problem");
            response = await client.PostAsync(ApiPath, model.ToJsonContent());
            response.EnsureSuccessStatusCode();
            Consumer.Instance.GetAll<IUserErrorReceived>()
                .Should().Contain(x =>
                    x.ReporterUserId == model.ReporterUserId &&
                    x.Description == model.Description &&
                    x.ExamId == model.ExamId &&
                    x.Image1FilePath == imagePath &&
                    x.ExamName == model.ExamName);
        }
    }
}
