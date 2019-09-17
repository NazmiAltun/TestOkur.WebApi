namespace TestOkur.WebApi.Integration.Tests
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using TestOkur.TestHelper.Extensions;
    using TestOkur.WebApi.Application.City;
    using TestOkur.WebApi.Integration.Tests.Common;
    using Xunit;

    public class CityControllerTests : Test
    {
        private const string ApiPath = "api/v1/cities";

        [Fact]
        public async Task Given_GetCities_Should_Return_AllCities_WithDistrictsInAlphabeticalOrder()
        {
            using (var testServer = await CreateAsync())
            {
                var client = testServer.CreateClient();
                var response = await client.GetAsync(ApiPath);
                var cities = await response.ReadAsync<IEnumerable<CityReadModel>>();
                CitiesAndDistrictsShouldNotBeEmpty(cities);
                CitiesShouldBeOrdered(cities);
                DistrictsShouldBeOrdered(cities);
            }
        }

        private void CitiesAndDistrictsShouldNotBeEmpty(IEnumerable<CityReadModel> cities)
        {
            cities.Should().NotBeEmpty()
                .And
                .NotContain(c => c.Districts == null || !c.Districts.Any())
                .And
                .NotContain(c => c.Districts.Select(d => d.Name).Any(string.IsNullOrEmpty));
        }

        private void CitiesShouldBeOrdered(IEnumerable<CityReadModel> cities)
        {
            cities.Select(c => c.Name)
                .Should()
                .BeInAscendingOrder(new TrStringComparer());
        }

        private void DistrictsShouldBeOrdered(IEnumerable<CityReadModel> cities)
        {
            foreach (var city in cities)
            {
                city.Districts
                    .Select(t => t.Name)
                    .Should()
                    .BeInAscendingOrder(new TrStringComparer());
            }
        }

        private class TrStringComparer : IComparer<object>
        {
            private static readonly CultureInfo Culture = new CultureInfo("tr-TR");

            public int Compare(object x, object y) =>
                string.Compare((string)x, (string)y, false, Culture);
        }
    }
}
