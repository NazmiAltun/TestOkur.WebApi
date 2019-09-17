namespace TestOkur.WebApi.Unit.Tests.Domain
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using TestOkur.Domain.Model.SmsModel;
    using Xunit;

    public class SmsCreditCalculatorTests
    {
        private readonly SmsCreditCalculator _smsCreditCalculator;

        public SmsCreditCalculatorTests()
        {
            _smsCreditCalculator = new SmsCreditCalculator();
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 1)]
        [InlineData(159, 1)]
        [InlineData(160, 2)]
        [InlineData(319, 2)]
        [InlineData(200, 2)]
        [InlineData(320, 3)]
        [InlineData(350, 3)]
        [InlineData(479, 3)]
        [InlineData(480, 4)]
        public void ShouldCalculateExpectedNumberOfCredits(int length, int expectedCredit)
        {
            var smsBody = string.Concat(Enumerable.Repeat("A", length));
            var calculatedCredits = _smsCreditCalculator.Calculate(smsBody);
            calculatedCredits.Should().Be(expectedCredit);
        }

        [Fact]
        public void GivenListOfSms_ShouldCalculateExpectedNumberOfCredits()
        {
            var list = new List<string>
            {
                string.Concat(Enumerable.Repeat("A", 159)),
                string.Concat(Enumerable.Repeat("A", 160)),
                string.Concat(Enumerable.Repeat("A", 479)),
                string.Concat(Enumerable.Repeat("A", 480)),
            };

            var credits = _smsCreditCalculator.Calculate(list);
            credits.Should().Be(10);
        }
    }
}
