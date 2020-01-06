namespace TestOkur.WebApi.Integration.Tests.User
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using TestOkur.Serializer;
    using TestOkur.TestHelper.Extensions;
    using TestOkur.WebApi.Application.User.Commands;
    using TestOkur.WebApi.Application.User.Queries;
    using TestOkur.WebApi.Integration.Tests.Common;

    public abstract class UserTest : Test
    {
        protected const string ApiPath = "api/v1/users";

        protected async Task<CreateUserCommand> CreateUserAsync(HttpClient client)
        {
            var command = GenerateCreateUserCommand();
            var response = await client.PostAsync(ApiPath, command.ToJsonContent());
            response.EnsureSuccessStatusCode();

            return command;
        }

        protected async Task<IReadOnlyCollection<UserReadModel>> GetUsersAsync(
            HttpClient client)
        {
            var response = await client.GetAsync(ApiPath);
            return await response.ReadAsync<IReadOnlyCollection<UserReadModel>>();
        }
    }
}
