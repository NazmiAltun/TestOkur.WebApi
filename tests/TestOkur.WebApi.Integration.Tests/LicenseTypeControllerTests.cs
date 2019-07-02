namespace TestOkur.WebApi.Integration.Tests
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using FluentAssertions;
	using TestOkur.TestHelper.Extensions;
	using TestOkur.WebApi.Application.LicenseType;
	using TestOkur.WebApi.Integration.Tests.Common;
	using Xunit;

	public class LicenseTypeControllerTests : Test
    {
        private const string ApiPath = "api/v1/license-types";

        [Fact]
        public async Task Given_Get_When_Requested_Then_ShouldReturn_Seeded_LicenseTypes()
        {
            using (var testServer = await CreateAsync())
            {
	            var client = testServer.CreateClient();
	            var response = await client.GetAsync(ApiPath);
	            response.EnsureSuccessStatusCode();
	            var list = await response.ReadAsync<IEnumerable<LicenseTypeReadModel>>();

	            list.Should().HaveCountGreaterThan(5);
	            list.SingleOrDefault(i => i.Name == "İLKOKUL-ORTAOKUL – (BİREYSEL)")
                    .Should()
                    .NotBeNull();
            }
        }
    }
}
