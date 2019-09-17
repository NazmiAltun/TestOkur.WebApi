namespace TestOkur.Notification.Infrastructure
{
    using Hangfire.Dashboard;
    using TestOkur.Notification.Configuration;

    public class BasicDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly HangfireConfiguration _hangfireConfiguration;

        public BasicDashboardAuthorizationFilter(HangfireConfiguration hangfireConfiguration)
        {
            _hangfireConfiguration = hangfireConfiguration;
        }

        public bool Authorize(DashboardContext context)
        {
            var username = context.Request.GetQuery("username");
            var password = context.Request.GetQuery("password");

            return username == _hangfireConfiguration.Username &&
                   password == _hangfireConfiguration.Password;
        }
    }
}
