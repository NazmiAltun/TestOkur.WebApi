namespace TestOkur.Notification.Integration.Tests.Common
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class TestStartup : Startup
	{
        public TestStartup(IConfiguration configuration, IHostingEnvironment environment)
            : base(configuration, environment)
        {
        }

        protected override void AddHostedServices(IServiceCollection services)
		{
		}
	}
}
