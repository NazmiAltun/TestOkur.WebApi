namespace TestOkur.Report.Integration.Tests.Common
{
	using System;
	using System.Security.Claims;
	using IdentityModel;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using TestOkur.Common;
	using TestOkur.TestHelper;

	public class TestStartup : Startup
	{
		public TestStartup(IConfiguration configuration)
			: base(configuration)
		{
		}

		protected override void AddHostedServices(IServiceCollection services)
		{
		}

		protected override void AddAuthentication(IServiceCollection services)
		{
			var subjectClaim = services.BuildServiceProvider()
				.GetService<Claim>();

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = "Test Scheme";
				options.DefaultChallengeScheme = "Test Scheme";
			}).AddTestAuth<TestAuthenticationOptions>(o =>
			{
				o.Identity = new ClaimsIdentity(
					new[]
					{
						new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", Guid.NewGuid().ToString()),
						new Claim(JwtClaimTypes.ClientId,  Clients.Public),
						new Claim(ClaimsIdentity.DefaultRoleClaimType, Roles.Admin),
						new Claim(ClaimsIdentity.DefaultRoleClaimType, Roles.Customer),
						subjectClaim ?? new Claim(JwtClaimTypes.Subject, Guid.NewGuid().ToString()),
					}, "test");
			});
		}
	}
}
