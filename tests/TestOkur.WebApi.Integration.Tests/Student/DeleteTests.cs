namespace TestOkur.WebApi.Integration.Tests.Student
{
	using FluentAssertions;
	using System.Linq;
	using System.Threading.Tasks;
	using TestOkur.Contracts.Student;
	using TestOkur.WebApi.Integration.Tests.Common;
	using Xunit;

	public class DeleteTests : StudentTest
	{
		[Fact]
		public async Task When_StudentExists_Should_Be_Deleted()
		{
			using (var testServer = await CreateWithUserAsync())
			{
				var client = testServer.CreateClient();
				var command = await CreateStudentAsync(client);
				var list = await GetListAsync(client);
				var id = list.First(s => s.StudentNumber == command.StudentNumber).Id;
				await client.DeleteAsync($"{ApiPath}/{id}");
				list = await GetListAsync(client);
				list.Should().NotContain(s => s.StudentNumber == command.StudentNumber);
				var @event = Consumer.Instance.GetFirst<IStudentDeleted>();
				@event.StudentId.Should().Be(id);
			}
		}
	}
}
