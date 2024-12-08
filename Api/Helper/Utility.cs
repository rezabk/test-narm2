using System.Security.Claims;

namespace Stap.Api.Helper;

public static class Utility
{
    public static int GetCurrentUserId(this HttpContext httpContext)
    {
        //int.TryParse(httpContext.User.Identity.Name, out int userId);
        int.TryParse(httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value, out var userId);
        return userId;
    }

    public static bool IsSuccessStatusCode(this int statusCode)
    {
        return statusCode >= 200 && statusCode < 300;
    }
}