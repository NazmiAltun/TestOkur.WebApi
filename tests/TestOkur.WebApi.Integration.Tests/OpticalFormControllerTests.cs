namespace TestOkur.WebApi.Integration.Tests
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using TestOkur.Domain.Model.LessonModel;
    using TestOkur.Domain.Model.OpticalFormModel;
    using TestOkur.TestHelper.Extensions;
    using TestOkur.WebApi.Application.OpticalForm;
    using TestOkur.WebApi.Integration.Tests.Common;
    using Xunit;

    public class OpticalFormControllerTests : Test
    {
        private const string ApiPath = "api/v1/optical-forms";

        [Fact]
        public async Task Given_Get_When_Requested_Then_OpticalForms_Shall_Return()
        {
            var client = (await GetTestServer()).CreateClient();
            var response = await client.GetAsync(ApiPath);
            var list = await response.ReadAsync<IEnumerable<OpticalFormTypeReadModel>>();

            var formTypeCodes = new[]
            {
                    OpticalFormTypes.Codes.Frm2ndGradeTrial,
                    OpticalFormTypes.Codes.Frm3rdGradeTrial,
                    OpticalFormTypes.Codes.Frm4thGradeTrial,
                    OpticalFormTypes.Codes.Frm20ABCD,
                    OpticalFormTypes.Codes.Frm30ABC,
                    OpticalFormTypes.Codes.Frm30ABCD,
                    OpticalFormTypes.Codes.Frm60ABCD,
                    OpticalFormTypes.Codes.Frm100ABCD,
                    OpticalFormTypes.Codes.FrmScholarship,
                    OpticalFormTypes.Codes.FrmTeog,
                    OpticalFormTypes.Codes.FrmLgs,
                    OpticalFormTypes.Codes.FrmLgsTwoPiece,
                    OpticalFormTypes.Codes.Frm20ABCDE,
                    OpticalFormTypes.Codes.Frm30ABCDE,
                    OpticalFormTypes.Codes.Frm60ABCDE,
                    OpticalFormTypes.Codes.Frm100ABCDE,
                    OpticalFormTypes.Codes.FrmTyt,
                    OpticalFormTypes.Codes.FrmAyt,
                    OpticalFormTypes.Codes.FrmAytLang,
                    OpticalFormTypes.Codes.FrmScholarshipHigh,
                    OpticalFormTypes.Codes.FrmSrc,
            };

            list.Select(f => f.Code)
                .Should().BeEquivalentTo(formTypeCodes);
            ShouldContainLgsExam(list);
            var random = list.Random();
            list.Should().NotContain(
                f => f.OpticalFormDefinitions == null ||
                     f.OpticalFormDefinitions.Count == 0 ||
                     f.MaxQuestionCount == 0);

            list.First(f => f.Code == OpticalFormTypes.Codes.FrmTyt)
                .FormLessonSections
                .Should().NotContain(f => f.FormPart == default);

            var path = random.OpticalFormDefinitions.Random().Path;
            var newPath = await client.DownloadAsync(path);
            Image.FromFile(newPath).Should().BeOfType<Bitmap>();
        }

        private void ShouldContainLgsExam(IEnumerable<OpticalFormTypeReadModel> list)
        {
            var lgs = list.First(f => f.Code == OpticalFormTypes.Codes.FrmLgs);
            var lessons = new[]
            {
                Lessons.Turkish,
                Lessons.Mathematics,
                Lessons.Science,
                Lessons.SocialScience,
                Lessons.Religion,
                Lessons.English,
            };

            lgs.FormLessonSections.Select(s => s.Lesson)
                .Should().BeEquivalentTo(lessons);
        }
    }
}
