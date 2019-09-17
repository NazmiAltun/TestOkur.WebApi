namespace TestOkur.WebApi.Integration.Tests.Lesson
{
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Xunit;

    public class DeleteTests : LessonTest
	{
		[Fact]
		public async Task ShouldDelete()
		{
			using (var testServer = await CreateWithUserAsync())
			{
				var client = testServer.CreateClient();
				var command = await CreateLessonAsync(client);
				var id = (await GetLessonListAsync(client))
					.First(l => l.Name == command.Name).Id;
				await client.DeleteAsync($"{ApiPath}/{id}");
				(await GetLessonListAsync(client)).Should()
					.NotContain(l => l.Name == command.Name);
			}
		}
	}
}
