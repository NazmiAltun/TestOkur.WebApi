namespace TestOkur.Report.Extensions
{
    using IdentityModel;
    using Microsoft.AspNetCore.Http;

    public static class IHttpContextAccessorExtensions
    {
        public static string GetUserId(this IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                return httpContextAccessor
                    .HttpContext.User
                    .FindFirst(JwtClaimTypes.Subject)
                    .Value;
            }
            catch
            {
                return default;
            }
        }
    }
}
