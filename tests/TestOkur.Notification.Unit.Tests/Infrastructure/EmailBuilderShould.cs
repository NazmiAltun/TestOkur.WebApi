using System.Threading.Tasks;
using FluentAssertions;
using RazorLight;
using TestOkur.Notification.Infrastructure;
using TestOkur.Notification.Models;
using Xunit;

namespace TestOkur.Notification.Unit.Tests.Infrastructure
{
    public class EmailBuilderShould
    {
        public class DummyModel
        {
            public DummyModel(string name)
            {
                Name = name;
            }

            private DummyModel()
            {
            }

            public string Name { get; }
        }

        [Fact]
        public async Task BuildMailMessage_When_ValidParametersProvided()
        {
            var templateService = new TemplateService(new RazorLightEngineBuilder()
                .UseMemoryCachingProvider()
                .Build());

            var email = await new EmailBuilder<DummyModel>(templateService)
                .WithTemplate(new Template("TEST", "TEST", "dummy.html"))
                .WithModel(new DummyModel("Tester"))
                .WithReceivers("tester@gmail.com")
                .Build();
            
            email.Should().NotBeNull();
        }
    }
}
