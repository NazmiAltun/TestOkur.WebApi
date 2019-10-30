﻿namespace TestOkur.WebApi.Integration.Tests.Error
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Http;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;
    using TestOkur.Contracts.Alert;
    using TestOkur.Infrastructure.Mvc.Extensions;
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
            using var testServer = await CreateWithUserAsync();
            var client = testServer.CreateClient();
            var imagePath = string.Empty;

            await using (var stream = File.OpenRead(Path.Combine("Error", "ss.png")))
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
                RandomGen.Next().ToString(),
                RandomGen.Next(),
                RandomGen.Next(),
                RandomGen.String(20),
                imagePath,
                null,
                null,
                "Houston!We've a problem");
            await client.PostAsync(ApiPath, model.ToJsonContent());
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
