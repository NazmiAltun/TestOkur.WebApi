namespace TestOkur.Sabit.Integration.Tests
{
    using FluentAssertions;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TestOkur.Sabit.Application.LicenseType;
    using TestOkur.TestHelper.Extensions;
    using Xunit;

    public class LicenseTypeTests : IClassFixture<WebApplicationFactory>
    {
        private const string ApiPath = "api/v1/license-types";

        private readonly WebApplicationFactory _factory;

        public LicenseTypeTests(WebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Given_Get_When_Requested_Then_ShouldReturn_Seeded_LicenseTypes()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync(ApiPath);
            response.EnsureSuccessStatusCode();
            var list = await response.ReadAsync<IEnumerable<LicenseType>>();

            list.Should().HaveCountGreaterThan(5);
            list.SingleOrDefault(i => i.Name == "İLKOKUL-ORTAOKUL – (BİREYSEL)")
                .Should()
                .NotBeNull();
        }
    }
}
