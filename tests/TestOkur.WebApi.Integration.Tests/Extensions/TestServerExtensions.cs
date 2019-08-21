namespace TestOkur.WebApi.Integration.Tests.Extensions
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Security.Claims;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.TestHost;
	using Microsoft.Extensions.DependencyInjection;
	using TestOkur.TestHelper.Extensions;
	using TestOkur.WebApi.Application.User.Queries;

	public static class TestServerExtensions
	{
		private const string UserApiPath = "api/v1/users";

		public static async Task<UserReadModel> GetCurrentUserInSession(this TestServer testServer)
		{
			var subjectId = (testServer.Host.Services
				.GetRequiredService(typeof(Claim)) as Claim).Value;
			var response = await testServer.CreateClient().GetAsync(UserApiPath);
			var users = await response.ReadAsync<IReadOnlyCollection<UserReadModel>>();

			return users.First(u => u.SubjectId == subjectId);
		}
	}
}
