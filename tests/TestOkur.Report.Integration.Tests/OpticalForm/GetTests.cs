namespace TestOkur.Report.Integration.Tests.OpticalForm
{
	using System.Linq;
	using System.Threading.Tasks;
	using FluentAssertions;
	using TestOkur.Optic.Form;
	using TestOkur.Report.Integration.Tests.Consumers;
	using Xunit;

	public class GetTests : ConsumerTest
	{
		[Fact]
		public async Task ShouldReturnAnswerKeyForms()
		{
			using (var testServer = Create())
			{
				var examId = await ExecuteExamCreatedConsumerAsync(testServer);
				var client = testServer.CreateClient();
				var examForms = await GetListAsync<AnswerKeyOpticalForm>(client, examId);
				examForms.SelectMany(e => e.Sections)
					.Should()
					.NotContain(s => string.IsNullOrEmpty(s.LessonName) ||
									 s.LessonId == default);
			}
		}
	}
}
