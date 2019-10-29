namespace TestOkur.Sabit.Integration.Tests
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc.Testing;
    using TestOkur.Sabit.Application.City;
    using TestOkur.TestHelper.Extensions;
    using Xunit;

    public class CityApiTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string ApiPath = "api/v1/cities";

        private readonly WebApplicationFactory<Startup> _factory;

        public CityApiTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Given_GetCities_Should_Return_AllCities_WithDistrictsInAlphabeticalOrder()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync(ApiPath);
            var cities = await response.ReadAsync<IEnumerable<City>>();
            CitiesAndDistrictsShouldNotBeEmpty(cities);
            CitiesShouldBeOrdered(cities);
            DistrictsShouldBeOrdered(cities);
        }

        private void CitiesAndDistrictsShouldNotBeEmpty(IEnumerable<City> cities)
        {
            cities.Should().NotBeEmpty()
                .And
                .NotContain(c => c.Districts == null || !c.Districts.Any())
                .And
                .NotContain(c => c.Districts.Select(d => d.Name).Any(string.IsNullOrEmpty));
        }

        private void CitiesShouldBeOrdered(IEnumerable<City> cities)
        {
            cities.Select(c => c.Name)
                .Should()
                .BeInAscendingOrder(new TrStringComparer());
        }

        private void DistrictsShouldBeOrdered(IEnumerable<City> cities)
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
