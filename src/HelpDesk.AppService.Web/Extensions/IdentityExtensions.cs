using System.Security.Claims;
using System.Security.Principal;

namespace HelpDesk.AppService.Web.Extensions
{
    public static class IdentityExtensions
    {
        #region Extension Methods

        public static string GetRole(this IIdentity identity)
            => ((ClaimsIdentity)identity)?.FindFirst(ClaimTypes.Role)?.Value;

        public static string GetEmail(this IIdentity identity)
            => ((ClaimsIdentity)identity)?.FindFirst(ClaimTypes.Email)?.Value;

        public static string GetFullName(this IIdentity identity)
            => string.Concat(((ClaimsIdentity)identity)?.FindFirst(ClaimTypes.Name)?.Value, " ", ((ClaimsIdentity)identity)?.FindFirst(ClaimTypes.Surname)?.Value);

        #endregion
    }
}
