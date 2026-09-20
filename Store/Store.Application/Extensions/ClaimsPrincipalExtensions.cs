using System.Security.Claims;

namespace Store.Application.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal principal)
        {
            var value = principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(value, out var id))
            {
                throw new UnauthorizedAccessException("Invalid user");
            }

            return id;
        }
    }
}
