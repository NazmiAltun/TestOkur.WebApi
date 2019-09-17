namespace TestOkur.WebApi.Integration.Tests.Contact
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using TestOkur.TestHelper;
    using TestOkur.WebApi.Application.Contact;
    using Xunit;

    public class EditTests : ContactTest
	{
		[Fact]
		public async Task ShoudEditInBulk()
		{
			const string ApiPath = "api/v1/contacts";

			using (var testServer = await CreateWithUserAsync())
			{
				var client = testServer.CreateClient();
				await CreateContactAsync(client);
				await CreateContactAsync(client);
				var list = await GetListAsync(client);

#pragma warning disable SA1118 // ParameterMustNotSpanMultipleLines
				var editCommand = new BulkEditContactsCommand(
					Guid.NewGuid(),
					new[]
					{
						new EditContactCommand(
							Guid.NewGuid(),
							RandomGen.String(10),
							RandomGen.String(10),
							RandomGen.Phone(),
							1 + RandomGen.Next(2),
							"Student,Teacher",
							list.First().Id),
						new EditContactCommand(
							Guid.NewGuid(),
							RandomGen.String(10),
							RandomGen.String(10),
							RandomGen.Phone(),
							1 + RandomGen.Next(2),
							"Student,Teacher",
							list.Last().Id),
					});

				await client.PutAsync(ApiPath, editCommand.ToJsonContent());
				list = await GetListAsync(client);
				list.Should().OnlyContain(c => c.Labels == "Student,Teacher");
			}
		}
	}
}
