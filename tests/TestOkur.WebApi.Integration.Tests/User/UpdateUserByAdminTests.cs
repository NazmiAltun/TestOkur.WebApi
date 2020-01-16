namespace TestOkur.WebApi.Integration.Tests.User
{
    using FluentAssertions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TestOkur.Serialization;
    using TestOkur.TestHelper;
    using TestOkur.TestHelper.Extensions;
    using TestOkur.WebApi.Application.User.Commands;
    using TestOkur.WebApi.Application.User.Queries;
    using Xunit;

    public class UpdateUserByAdminTests : UserTest
    {
        [Fact]
        public async Task ShouldUpdateUser()
        {
            var testServer = await GetTestServer();
            var client = testServer.CreateClient();
            var createUserCommand = await CreateUserAsync(client);
            var response = await client.GetAsync(ApiPath);
            var users = await response.ReadAsync<IReadOnlyCollection<UserReadModel>>();
            var user = users.First(u => u.Email == createUserCommand.Email);
            var command = new UpdateUserByAdminCommand(
                Guid.NewGuid(),
                user.Id,
                RandomGen.String(5),
                RandomGen.String(5),
                user.SubjectId,
                RandomGen.String(100),
                "5544205163",
                81,
                23,
                $"{RandomGen.String(120)}@hotmail.com",
                RandomGen.Next(10),
                RandomGen.Next(300) + 100,
                true,
                RandomGen.Next(4) + 1,
                DateTime.Now.AddDays(200),
                RandomGen.String(100),
                true);
            response = await client.PostAsync(
                $"{ApiPath}/update-by-admin",
                command.ToJsonContent());
            response.EnsureSuccessStatusCode();
            response = await client.GetAsync(ApiPath);
            users = await response.ReadAsync<IReadOnlyCollection<UserReadModel>>();
            users.First(u => u.Id == user.Id)
                .Should().Match<UserReadModel>(
                    u => u.Email == command.Email &&
                         u.SchoolName == command.SchoolName &&
                         u.FirstName == command.FirstName &&
                         u.LastName == command.LastName &&
                         u.CityId == command.CityId &&
                         u.DistrictId == command.DistrictId &&
                         u.Phone == command.MobilePhone);
        }
    }
}
