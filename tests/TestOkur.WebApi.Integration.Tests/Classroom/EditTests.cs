namespace TestOkur.WebApi.Integration.Tests.Classroom
{
	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using FluentAssertions;
	using TestOkur.Common;
	using TestOkur.Contracts.Classroom;
	using TestOkur.TestHelper.Extensions;
	using TestOkur.WebApi.Application.Classroom;
	using TestOkur.WebApi.Integration.Tests.Common;
	using Xunit;

	public class EditTests : ClassroomTest
	{
		[Fact]
		public async Task WhenExistingValuesPosted_Then_BadRequestShouldBeReturned()
		{
			using (var testServer = await CreateWithUserAsync())
			{
				var client = testServer.CreateClient();
				var command1 = await CreateClassroomAsync(client);
				var command2 = await CreateClassroomAsync(client);
				var list = await GetListAsync(client);
				var id = list.First(c => c.Grade == command1.Grade && c.Name == command1.Name).Id;
				var editCommand = new EditClassroomCommand(Guid.NewGuid(), id, command2.Name, command2.Grade);
				var response = await client.PutAsync(ApiPath, editCommand.ToJsonContent());
				await response.Should().BeBadRequestAsync(ErrorCodes.ClassroomExists);
			}
		}

		[Fact]
		public async Task WhenNotExistingValuesPosted_Then_ShouldEdit()
		{
			using (var testServer = await CreateWithUserAsync())
			{
				var client = testServer.CreateClient();
				var command = await CreateClassroomAsync(client);
				var list = await GetListAsync(client);
				var id = list.First(c => c.Grade == command.Grade && c.Name == command.Name).Id;
				var editCommand = new EditClassroomCommand(Guid.NewGuid(), id, Random.RandomString(2), command.Grade);
				await client.PutAsync(ApiPath, editCommand.ToJsonContent());
				list = await GetListAsync(client);

				list.Should().Contain(c => c.Name == editCommand.NewName && c.Grade == command.Grade)
					.And
					.NotContain(c => c.Name == command.Name && c.Grade == command.Grade);
				var @event = Consumer.Instance.GetFirst<IClassroomUpdated>();
				@event.ClassroomId.Should().Be(id);
				@event.Grade.Should().Be(editCommand.NewGrade);
				@event.Name.Should().Be(editCommand.NewName);
			}
		}
	}
}
