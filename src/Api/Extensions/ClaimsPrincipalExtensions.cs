using Domain.Enums;
using System.Security.Claims;

namespace Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int? GetUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var id) ? id : null;
        }

        public static UserRoles GetRole(this ClaimsPrincipal principal)
        {
            var roleClaim = principal.FindFirst(ClaimTypes.Role);
            if (roleClaim == null || !Enum.TryParse<UserRoles>(roleClaim.Value, out var role))
            {
                return UserRoles.User;
            }
            return role;
        }
    }
}
