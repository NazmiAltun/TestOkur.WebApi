namespace TestOkur.Notification.Infrastructure
{
	using Hangfire.Dashboard;

	public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
	{
	    public bool Authorize(DashboardContext context)
	    {
		    return true;
	    }
	}
}
