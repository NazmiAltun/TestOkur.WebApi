namespace TestOkur.WebApi.Integration.Tests.Contact
{
	using System;
	using System.Threading.Tasks;
	using FluentAssertions;
	using TestOkur.Common;
	using TestOkur.TestHelper.Extensions;
	using Xunit;

	public class CreateTests : ContactTest
	{
		[Fact]
		public async Task When_PhoneExists_Then_ShouldReturnBadRequest()
		{
			const string ApiPath = "api/v1/contacts";

			using (var testServer = await CreateWithUserAsync())
			{
				var client = testServer.CreateClient();
				var command = await CreateContactAsync(client);
				var response = await client.PostAsync(ApiPath, command.ToJsonContent());
				await response.Should().BeBadRequestAsync(ErrorCodes.ContactExists);
			}
		}

		[Fact]
		public async Task When_StudentWithContactsPosted_Then_StudentContactsWithFullDetails_Shoud_BeReturned()
		{
			using (var testServer = await CreateWithUserAsync())
			{
				var client = testServer.CreateClient();
				var command = await CreateStudentAsync(client);
				var list = await GetListAsync(client);

				foreach (var contact in command.Contacts)
				{
					list.Should().Contain(c => !string.IsNullOrEmpty(c.ClassroomName) &&
											   c.Grade > 0 &&
					                           c.FirstName == contact.FirstName &&
					                           c.LastName == contact.LastName &&
					                           c.Phone == contact.Phone &&
					                           !string.IsNullOrEmpty(c.ContactTypeName));
				}
			}
		}

		[Fact]
		public async Task When_ValidValuesArePosted_Then_ContactShouldBeCreated()
		{
			using (var testServer = await CreateWithUserAsync())
			{
				var client = testServer.CreateClient();
				var command = await CreateContactAsync(client);
				(await GetListAsync(client)).Should()
					.Contain(l => l.Phone == command.Phone &&
					              l.FirstName == command.FirstName &&
					              l.LastName == command.LastName &&
					              l.ContactType == command.ContactType);
			}
		}
	}
}
