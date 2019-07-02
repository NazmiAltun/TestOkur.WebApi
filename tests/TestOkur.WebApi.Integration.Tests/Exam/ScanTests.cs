namespace TestOkur.WebApi.Integration.Tests.Exam
{
	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using FluentAssertions;
	using Microsoft.EntityFrameworkCore;
	using TestOkur.Data;
	using TestOkur.WebApi.Application.Scan;
	using Xunit;

	public class ScanTests : ExamTest
	{
		protected new const string ApiPath = "api/v1/scan-sessions";

		[Fact]
		public async Task ShouldStartAndEnd()
		{
			using (var testServer = await CreateWithUserAsync())
			{
				var client = testServer.CreateClient();
				var exam = await CreateExamAsync(client);
				var exams = await GetExamListAsync(client);

				var startCommand = new StartScanSessionCommand(
					Guid.NewGuid(),
					exams.First().Id,
					true,
					false,
					"USB Camera");
				var response = await client.PostAsync(ApiPath, startCommand.ToJsonContent());
				response.EnsureSuccessStatusCode();
				var endCommand = new EndScanSessionCommand(startCommand.Id, Random.Next());
				response = await client.PutAsync(ApiPath, endCommand.ToJsonContent());
				response.EnsureSuccessStatusCode();

				var dbContext = testServer.Host.Services.GetService(typeof(ApplicationDbContext))
					as ApplicationDbContext;
				var session = await dbContext.ExamScanSessions.FirstAsync(e => e.ReportId == startCommand.Id);
				session.ScannedStudentCount.Should().Be(endCommand.ScannedStudentCount);
				session.Source.Should().Be(startCommand.Source);
				session.ByCamera.Should().Be(startCommand.ByCamera);
				session.ByFile.Should().Be(startCommand.ByFile);
			}
		}
	}
}
