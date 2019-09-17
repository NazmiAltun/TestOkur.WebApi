namespace TestOkur.WebApi.Integration.Tests.Contact
{
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Xunit;

    public class DeleteTests : ContactTest
	{
		[Fact]
		public async Task ShouldDelete()
		{
			const string ApiPath = "api/v1/contacts";

			using (var testServer = await CreateWithUserAsync())
			{
				var client = testServer.CreateClient();
				var command = await CreateContactAsync(client);
				var id = (await GetListAsync(client))
					.First(c => c.Phone == command.Phone).Id;
				await client.DeleteAsync($"{ApiPath}/{id}");
				(await GetListAsync(client)).Should()
					.NotContain(l => l.Phone == command.Phone);
			}
		}
	}
}
