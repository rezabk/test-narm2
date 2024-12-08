using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Common.Enums;
using Common.Enums.RolesManagment;
using Common.Enums.UserEnum;

namespace Common.GetClaimUtils;

public class RoleValueUser
{
    public string Value { get; set; }
    public string Description { get; set; }
}

public class ClaimValueUser
{
    public string ClaimType { get; set; }
    public string Description { get; set; }
    public string CliameValueTitle { get; set; }
    public string CliameValue { get; set; }
    public string Attribute { get; set; }
}

public static class ClaimUtils
{
    //public static ClaimsPrincipal GetUserAuthorized()
    //{
    //    var httpContextAccessor = (IHttpContextAccessor)validationContext.GetService(typeof(IHttpContextAccessor));
    //    var user = httpContextAccessor.HttpContext.User;
    //}
    public static List<string> GetUserClaimsValue(this ClaimsPrincipal principal, string claimType)
    {
        if (principal == null)
            throw new ArgumentNullException(nameof(principal));

        return principal.Claims.Where(x => x.Type == claimType).Select(x => x.Value).ToList();
    }

    public static List<string> GetUserRolesValue(this ClaimsPrincipal principal)
    {
        if (principal == null)
            throw new ArgumentNullException(nameof(principal));

        return principal.Claims.Where(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
            .Select(x => x.Value).ToList();
    }

    public static string GetUserName(this ClaimsPrincipal principal)
    {
        if (principal == null)
            throw new ArgumentNullException(nameof(principal));

        return principal.Claims.FirstOrDefault(x => x.Type == nameof(ClaimUserEnum.preferred_username))?.Value;
    }

    public static string GetNameOfUser(this ClaimsPrincipal principal)
    {
        if (principal == null)
            throw new ArgumentNullException(nameof(principal));

        return principal.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
    }

    public static string GetUserLastName(this ClaimsPrincipal principal)
    {
        if (principal == null)
            throw new ArgumentNullException(nameof(principal));

        return principal.Claims
            .FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname")?.Value;
    }

    public static int GetUserId(this ClaimsPrincipal principal)
    {
        if (principal == null)
            throw new ArgumentNullException(nameof(principal));

        return int.Parse(principal.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value);
    }

    public static string GetPhoneNumber(this ClaimsPrincipal principal)
    {
        if (principal == null)
            throw new ArgumentNullException(nameof(principal));

        return principal.Claims.FirstOrDefault(x => x.Type == "mobilenumber")?.Value;
    }


    public static string GetNationalCode(this ClaimsPrincipal claimsPrincipal)
    {
        try
        {
            return claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "Username")?.Value;
        }
        catch (Exception e)
        {
            return string.Empty;
        }
    }

    public static string GetFirstName(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
    }

    public static string GetLastName(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.Claims
            .FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname")?.Value;
    }

    public static List<RoleValueUser> GetRoles(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal
            .Claims
            .Where(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
            .Select(x => new RoleValueUser
            {
                Value = x.Value,
                Description = ((UserRolesEnum)System.Enum.Parse(typeof(UserRolesEnum), x.Value)).GetEnumDescription()
            }).ToList();
    }
}