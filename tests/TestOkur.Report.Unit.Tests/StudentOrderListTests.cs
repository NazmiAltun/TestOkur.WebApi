namespace TestOkur.Report.Unit.Tests
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Domain;
    using Xunit;

    public class StudentOrderListTests
    {
        private readonly Random _random = new Random();

        [Fact]
        public void ShouldCalculateExpectedly()
        {
            var a = Generate(1, 1, 1, 80);
            var b = Generate(1, 1, 1, 80);
            var c = Generate(1, 1, 1, 85);
            var d = Generate(2, 1, 1, 85);
            var e = Generate(3, 2, 1, 75);
            var f = Generate(4, 2, 2, 75);
            var g = Generate(4, 2, 2, 65);
            var h = Generate(5, 3, 3, 80);
            var i = Generate(5, 3, 3, 85);

            var forms = new List<StudentOpticalForm>()
            {
                a, b, c, d, e, f, g, h, i,
            };

            var list = new StudentOrderList("Net", forms, x => x.Net);

            list.GetStudentOrder(a).Should()
                .Match<StudentOrder>(
                    x => x.GeneralOrder == 2 &&
                         x.ClassroomOrder == 2 &&
                         x.SchoolOrder == 2 &&
                         x.DistrictOrder == 2);
            list.GetStudentOrder(b).Should()
                .Match<StudentOrder>(
                    x => x.GeneralOrder == 2 &&
                         x.ClassroomOrder == 2 &&
                         x.SchoolOrder == 2 &&
                         x.DistrictOrder == 2);
            list.GetStudentOrder(c).Should()
                .Match<StudentOrder>(
                    x => x.GeneralOrder == 1 &&
                         x.ClassroomOrder == 1 &&
                         x.SchoolOrder == 1 &&
                         x.DistrictOrder == 1);
            list.GetStudentOrder(d).Should()
                .Match<StudentOrder>(
                    x => x.GeneralOrder == 1 &&
                         x.ClassroomOrder == 1 &&
                         x.SchoolOrder == 1 &&
                         x.DistrictOrder == 1);
            list.GetStudentOrder(e).Should()
                .Match<StudentOrder>(
                    x => x.GeneralOrder == 3 &&
                         x.ClassroomOrder == 1 &&
                         x.SchoolOrder == 1 &&
                         x.DistrictOrder == 3);
            list.GetStudentOrder(e).Should()
                .Match<StudentOrder>(
                    x => x.GeneralOrder == 3 &&
                         x.ClassroomOrder == 1 &&
                         x.SchoolOrder == 1 &&
                         x.DistrictOrder == 3);
            list.GetStudentOrder(f).Should()
                .Match<StudentOrder>(
                    x => x.GeneralOrder == 3 &&
                         x.ClassroomOrder == 1 &&
                         x.SchoolOrder == 1 &&
                         x.DistrictOrder == 1);
            list.GetStudentOrder(g).Should()
                .Match<StudentOrder>(
                    x => x.GeneralOrder == 4 &&
                         x.ClassroomOrder == 2 &&
                         x.SchoolOrder == 2 &&
                         x.DistrictOrder == 2);
            list.GetStudentOrder(h).Should()
                .Match<StudentOrder>(
                    x => x.GeneralOrder == 2 &&
                         x.ClassroomOrder == 2 &&
                         x.SchoolOrder == 2 &&
                         x.DistrictOrder == 2);
            list.GetStudentOrder(i).Should()
                .Match<StudentOrder>(
                    x => x.GeneralOrder == 1 &&
                         x.ClassroomOrder == 1 &&
                         x.SchoolOrder == 1 &&
                         x.DistrictOrder == 1);
        }

        private StudentOpticalForm Generate(int classroomId, int userId, int districtId, float net)
        {
            return new StudentOpticalForm
            {
                StudentId = _random.Next(),
                ClassroomId = classroomId,
                UserId = userId.ToString(),
                SchoolId = userId,
                DistrictId = districtId,
                Sections = new List<StudentOpticalFormSection>()
                {
                    new StudentOpticalFormSection(new AnswerKeyOpticalFormSection(1, "Math"))
                    {
                        Net = net,
                    },
                },
            };
        }
    }
}
