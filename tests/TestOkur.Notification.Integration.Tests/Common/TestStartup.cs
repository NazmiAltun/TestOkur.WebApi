namespace TestOkur.Notification.Integration.Tests.Common
{
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;

	public class TestStartup : Startup
	{
		public TestStartup(IConfiguration configuration)
			: base(configuration)
		{
		}

		protected override void AddHostedServices(IServiceCollection services)
		{
		}
	}
}
