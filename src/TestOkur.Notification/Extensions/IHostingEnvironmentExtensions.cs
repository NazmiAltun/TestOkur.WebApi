namespace TestOkur.Notification.Extensions
{
	using Microsoft.AspNetCore.Hosting;

	public static class IHostingEnvironmentExtensions
	{
		public static bool IsProd(this IHostingEnvironment hostingEnvironment)
		{
			return hostingEnvironment.EnvironmentName == "prod";
		}
	}
}
