namespace TestOkur.Report.Unit.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using TestOkur.Report.Domain;
    using Xunit;

    public class SchoolOrderListTests
    {
        [Fact]
        public void Should_Evaluate_Orders_Correctly()
        {
            var results = GetTestData().ToList();
            var orderList = new SchoolOrderList(results, r => r.ScoreAverage);
            foreach (var result in results)
            {
                result.CityOrder = orderList.GetCityOrder(result);
                result.DistrictOrder = orderList.GetDistrictOrder(result);
                result.GeneralOrder = orderList.GetGeneralOrder(result);
            }

            results.ElementAt(0).CityOrder.Should().Be(1);
            results.ElementAt(0).DistrictOrder.Should().Be(1);
            results.ElementAt(0).GeneralOrder.Should().Be(2);
            results.ElementAt(1).CityOrder.Should().Be(2);
            results.ElementAt(1).DistrictOrder.Should().Be(2);
            results.ElementAt(1).GeneralOrder.Should().Be(3);
            results.ElementAt(2).CityOrder.Should().Be(2);
            results.ElementAt(2).DistrictOrder.Should().Be(2);
            results.ElementAt(2).GeneralOrder.Should().Be(5);
            results.ElementAt(3).CityOrder.Should().Be(1);
            results.ElementAt(3).DistrictOrder.Should().Be(1);
            results.ElementAt(3).GeneralOrder.Should().Be(4);
            results.ElementAt(4).CityOrder.Should().Be(1);
            results.ElementAt(4).DistrictOrder.Should().Be(1);
            results.ElementAt(4).GeneralOrder.Should().Be(1);
        }

        private IEnumerable<SchoolResult> GetTestData()
        {
            var all = new List<SchoolResult>()
            {
                new SchoolResult
                {
                    CityId = 31,
                    DistrictId = 750,
                    ScoreAverage = 220,
                },
                new SchoolResult
                {
                    CityId = 31,
                    DistrictId = 750,
                    ScoreAverage = 210,
                },
                new SchoolResult
                {
                    CityId = 33,
                    DistrictId = 339,
                    ScoreAverage = 204,
                },
                new SchoolResult
                {
                    CityId = 33,
                    DistrictId = 339,
                    ScoreAverage = 205,
                },
                new SchoolResult
                {
                    CityId = 35,
                    DistrictId = 569,
                    ScoreAverage = 300,
                },
            };

            foreach (var item in all)
            {
                yield return item;
            }
        }
    }
}
