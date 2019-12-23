namespace TestOkur.Report.Extensions
{
    using IdentityModel;
    using Microsoft.AspNetCore.Http;
    using TestOkur.Common;

    public static class IHttpContextAccessorExtensions
    {
        public static bool CheckIfAdmin(this IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                return httpContextAccessor
                           .HttpContext.User
                           .FindFirst(JwtClaimTypes.Role)
                           .Value == Roles.Admin;
            }
            catch
            {
                return false;
            }
        }
    }
}
