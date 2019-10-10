namespace TestOkur.Notification.Extensions
{
    using Microsoft.AspNetCore.Hosting;

    public static class IWebHostEnvironmentExtensions
    {
        public static bool IsProd(this IWebHostEnvironment hostingEnvironment)
        {
            return hostingEnvironment.EnvironmentName == "prod";
        }
    }
}
