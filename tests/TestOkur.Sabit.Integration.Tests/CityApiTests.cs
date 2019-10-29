namespace TestOkur.Sabit.Integration.Tests
{
    using System.Net;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Xunit;

    public class CityApiTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public CityApiTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }
    }
}
