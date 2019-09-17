namespace TestOkur.WebApi.Integration.Tests.Lesson.Unit
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using TestOkur.Common;
    using TestOkur.TestHelper.Extensions;
    using TestOkur.WebApi.Application.Lesson.Commands;
    using Xunit;

    public class EditTests : UnitTest
	{
		[Fact]
		public async Task When_UnitExists_Then_BadRequestShouldBeReturned()
		{
			using (var testServer = await CreateWithUserAsync())
			{
				var client = testServer.CreateClient();
				var command1 = await CreateUnitAsync(client);
				var command2 = await CreateUnitAsync(client);
				var list = await GetUnitListAsync(client);
				var id = list.First(c => c.Name == command2.Name).Id;
				var editCommand = new EditUnitCommand(
					Guid.NewGuid(),
					id,
					command1.Name);
				var response = await client.PutAsync(ApiPath, editCommand.ToJsonContent());
				await response.Should().BeBadRequestAsync(ErrorCodes.UnitExists);
			}
		}

		[Fact]
		public async Task When_Valid_Name_Provided_Should_Edit()
		{
			using (var testServer = await CreateWithUserAsync())
			{
				var client = testServer.CreateClient();
				var command1 = await CreateUnitAsync(client);
				var list = await GetUnitListAsync(client);
				var id = list.First(c => c.Name == command1.Name).Id;
				var editCommand = new EditUnitCommand(
					Guid.NewGuid(),
					id,
					Random.RandomString(100));
				await client.PutAsync(ApiPath, editCommand.ToJsonContent());
				list = await GetUnitListAsync(client);
				list.Should().Contain(u => u.Name == editCommand.NewName &&
				                           u.Id == id);
			}
		}
	}
}
