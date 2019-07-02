namespace TestOkur.WebApi.Integration.Tests.Lesson.Unit
{
	using System;
	using System.Threading.Tasks;
	using FluentAssertions;
	using TestOkur.Common;
	using TestOkur.TestHelper.Extensions;
	using Xunit;

	public class CreateTests : UnitTest
	{
		[Fact]
		public async Task When_UnitExists_Then_BadRequestShouldBeReturned()
		{
			using (var testServer = await CreateWithUserAsync())
			{
				var client = testServer.CreateClient();
				var command = await CreateUnitAsync(client);
				var response = await client.PostAsync(ApiPath, command.ToJsonContent());
				await response.Should().BeBadRequestAsync(ErrorCodes.UnitExists);
			}
		}

		[Fact]
		public async Task When_ValidValuesArePosted_Then_UnitShouldBeCreated()
		{
			using (var testServer = await CreateWithUserAsync())
			{
				var client = testServer.CreateClient();
				var command = await CreateUnitAsync(client);
				(await GetUnitListAsync(client)).Should()
					.Contain(l => l.Name == command.Name &&
					              l.Grade == command.Grade &&
					              l.LessonId == command.LessonId);
			}
		}
	}
}
