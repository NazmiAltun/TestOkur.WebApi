namespace TestOkur.WebApi.Integration.Tests
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Threading.Tasks;
    using FluentAssertions;
    using TestOkur.WebApi.Integration.Tests.Common;
    using Xunit;

    public class CaptchaControllerTests : Test
    {
        private const string ApiPath = "api/v1/captcha";

        [Fact]
        public async Task When_Requested_CaptchaService_Should_Generate_Captcha()
        {
            var id = Guid.NewGuid();
            var client = (await GetTestServer()).CreateClient();
            var response = await client.GetAsync($"{ApiPath}/{id}");
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            using (var image = Image.FromStream(stream))
            {
                image.RawFormat.Should().Be(ImageFormat.Png);
            }
        }
    }
}
