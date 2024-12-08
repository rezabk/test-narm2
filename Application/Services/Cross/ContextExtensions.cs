using Application.DTO;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Cross;

public static class HttpAccessorExtensions
{
    public static string GetCurrentUserName(this IHttpContextAccessor accessor)
    {
        var username = accessor?.HttpContext?.User?.FindFirst("Username");
        return username != null ? username.Value : "anonymous";
    }

    public static string GetCurrentUserNationalCode(this IHttpContextAccessor accessor)
    {
        var nationalCode = accessor?.HttpContext?.User?.FindFirst("nationalcode");
        return nationalCode != null ? nationalCode.Value : "anonymous";
    }

    public static string GetCurrentUserFirstName(this IHttpContextAccessor accessor)
    {
        var firstName =
            accessor?.HttpContext?.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname");
        return firstName != null ? firstName.Value : "anonymous";
    }

    public static string GetCurrentUserLastName(this IHttpContextAccessor accessor)
    {
        var lastName =
            accessor?.HttpContext?.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname");
        return lastName != null ? lastName.Value : "anonymous";
    }

    public static int GetCurrentUserIdentityId(this IHttpContextAccessor accessor)
    {
        var stringId = accessor?.HttpContext?.User.FindFirst("userid").Value;
        return int.Parse(stringId);
    }

    public static List<string> GetCurrentUserRoles(this IHttpContextAccessor accessor)
    {
        var roles = accessor?.HttpContext?.User?.FindAll("role").Select(s => s.Value).ToList();
        return roles;
    }

    public static bool IsUserInRole(this IHttpContextAccessor accessor, string userRoleEnumString)
    {
        var allUserRoles = accessor.GetCurrentUserRoles();
        return allUserRoles.Contains(userRoleEnumString);
    }

    public static CurrentUserInformationDTO GetCurrentUserInformation(this IHttpContextAccessor accessor)
    {
        return new CurrentUserInformationDTO
        {
            UserIdentityId = accessor.GetCurrentUserIdentityId(),
            FirstName = accessor.GetCurrentUserFirstName(),
            LastName = accessor.GetCurrentUserLastName(),
            UserName = accessor.GetCurrentUserName(),
            NationalCode = accessor.GetCurrentUserNationalCode()
        };
    }
}