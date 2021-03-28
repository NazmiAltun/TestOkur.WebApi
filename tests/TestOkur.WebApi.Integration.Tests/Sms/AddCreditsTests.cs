namespace TestOkur.WebApi.Integration.Tests.Sms
{
    using FluentAssertions;
    using System;
    using System.Threading.Tasks;
    using TestOkur.Contracts.Sms;
    using TestOkur.Serialization;
    using TestOkur.TestHelper;
    using TestOkur.TestHelper.Extensions;
    using TestOkur.WebApi.Application.Sms.Commands;
    using TestOkur.WebApi.Integration.Tests.Common;
    using TestOkur.WebApi.Integration.Tests.User;
    using Xunit;

    public class AddCreditsTests : UserTest
    {
        private new const string ApiPath = "api/v1/sms/add-credits";

       [Fact(Skip = "Fix later")]
        public async Task ShouldAddCredits()
        {
            var testServer = await GetTestServer();
            var client = testServer.CreateClient();
            var model = await CreateUserAsync(client);
            var additionAmount = RandomGen.Next(1000);
            var response = await client.GetAsync($"api/v1/users/{model.Email}");
            var user = await response.ReadAsync<Domain.Model.UserModel.User>();
            var command = new AddSmsCreditsCommand(Guid.NewGuid(), (int)user.Id, additionAmount, true);
            response = await client.PostAsync($"{ApiPath}", command.ToJsonContent());
            response.EnsureSuccessStatusCode();
            response = await client.GetAsync($"api/v1/users/{model.Email}");
            user = await response.ReadAsync<Domain.Model.UserModel.User>();
            user.SmsBalance.Should().Be(additionAmount);
            Consumer.Instance.GetAll<ISmsCreditAdded>()
                .Should().Contain(x =>
                    x.Amount == command.Amount &&
                    x.Email == user.Email &&
                    x.Phone == user.Phone);
        }
    }
}
