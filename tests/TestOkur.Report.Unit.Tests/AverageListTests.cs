namespace TestOkur.Report.Unit.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Domain;
    using TestOkur.TestHelper.Extensions;
    using Xunit;

    public class AverageListTests
    {
        private readonly Random _random = new Random();

        [Fact]
        public void ShouldCalculateAverages()
        {
            var a = Generate(1, 1, 1, 1, 80);
            var b = Generate(1, 1, 1, 1, 80);
            var c = Generate(1, 1, 1, 1, 85);
            var d = Generate(2, 1, 1, 1, 85);
            var e = Generate(3, 2, 1, 1, 75);
            var f = Generate(4, 2, 2, 1, 75);
            var g = Generate(4, 2, 2, 1, 65);
            var h = Generate(5, 3, 3, 2, 80);
            var i = Generate(5, 3, 3, 2, 85);

            var forms = new List<StudentOpticalForm>()
            {
                a, b, c, d, e, f, g, h, i,
            };

            var list = new AverageList("NET", forms, s => s.Net);
            var average = list.GetGeneralAverage(a.Sections.First().LessonName);
            average.Should().Be(78.89f);
            average = list.GetDistrictAverage(a.Sections.First().LessonName, 1);
            average.Should().Be(81);
            average = list.GetCityAverage(a.Sections.First().LessonName, 1);
            average.Should().Be(77.86f);
            average = list.GetClassroomAverage(a.Sections.First().LessonName, 4);
            average.Should().Be(70);
            average = list.GetSchoolAverage(a.Sections.First().LessonName, 2);
            average.Should().Be(71.67f);
        }

        private StudentOpticalForm Generate(int classroomId, int userId, int districtId, int cityId, float net)
        {
            return new StudentOpticalForm
            {
                StudentId = _random.Next(),
                ClassroomId = classroomId,
                SchoolId = userId,
                UserId = userId.ToString(),
                DistrictId = districtId,
                CityId = cityId,
                Sections = new List<StudentOpticalFormSection>()
                {
                    new StudentOpticalFormSection(new AnswerKeyOpticalFormSection(1, "Math"))
                    {
                        Net = net,
                    },
                    new StudentOpticalFormSection(new AnswerKeyOpticalFormSection(2, _random.RandomString(10)))
                    {
                        Net = net,
                    },
                },
            };
        }
    }
}
