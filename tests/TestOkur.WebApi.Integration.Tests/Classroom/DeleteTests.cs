namespace TestOkur.WebApi.Integration.Tests.Classroom
{
	using FluentAssertions;
	using System.Linq;
	using System.Threading.Tasks;
	using TestOkur.Contracts.Classroom;
	using TestOkur.WebApi.Integration.Tests.Common;
	using Xunit;

	public class DeleteTests : ClassroomTest
	{
		[Fact]
		public async Task ShouldDelete()
		{
			using (var testServer = await CreateWithUserAsync())
			{
				var client = testServer.CreateClient();
				var command = await CreateClassroomAsync(client);
				var id = (await GetListAsync(client))
					.First(c => c.Grade == command.Grade && c.Name == command.Name).Id;
				await client.DeleteAsync($"{ApiPath}/{id}");
				(await GetListAsync(client)).Should()
					.NotContain(l => l.Grade == command.Grade && l.Name == command.Name);
				var @event = Consumer.Instance.GetFirst<IClassroomDeleted>();
				@event.ClassroomId.Should().Be(id);
			}
		}
	}
}
