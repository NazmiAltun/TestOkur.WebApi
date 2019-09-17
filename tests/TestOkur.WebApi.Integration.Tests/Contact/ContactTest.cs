namespace TestOkur.WebApi.Integration.Tests.Contact
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using TestOkur.Domain.Model.StudentModel;
    using TestOkur.TestHelper;
    using TestOkur.TestHelper.Extensions;
    using TestOkur.WebApi.Application.Contact;
    using TestOkur.WebApi.Integration.Tests.Student;

    public class ContactTest : StudentTest
	{
		protected async Task<CreateContactCommand> CreateContactAsync(HttpClient client)
		{
			const string ApiPath = "api/v1/contacts";
			var command = new CreateContactCommand(
				Guid.NewGuid(),
				Random.RandomString(10),
				Random.RandomString(10),
				RandomGen.Phone(),
				(int)ContactType.Directory.Id);

			var response = await client.PostAsync(ApiPath, command.ToJsonContent());
			response.EnsureSuccessStatusCode();

			return command;
		}

		protected new async Task<IEnumerable<ContactReadModel>> GetListAsync(HttpClient client)
		{
			const string ApiPath = "api/v1/contacts";

			var response = await client.GetAsync(ApiPath);
			return await response.ReadAsync<IEnumerable<ContactReadModel>>();
		}
	}
}
