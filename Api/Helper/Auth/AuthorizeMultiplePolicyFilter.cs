using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Stap.Api.Helper.Auth;

public class AuthorizeMultiplePolicyFilter : IAsyncAuthorizationFilter
{
    private readonly IAuthorizationService _authorization;

    public AuthorizeMultiplePolicyFilter(string policy, bool isAll, IAuthorizationService authorization)
    {
        Policy = policy;
        _authorization = authorization;
        IsAll = isAll;
    }

    public string Policy { get; }
    public bool IsAll { get; set; }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var policies = Policy.Split(";").ToList();
        var isAuthenticated = context.HttpContext.User.Identity.IsAuthenticated;
        if (IsAll)
        {
            foreach (var policy in policies)
            {
                var authorized = await _authorization.AuthorizeAsync(context.HttpContext.User, policy);
                if (isAuthenticated && !authorized.Succeeded)
                {
                    context.Result = new StatusCodeResult(403);
                    return;
                }

                context.Result = new UnauthorizedResult();
                return;
            }
        }
        else
        {
            foreach (var policy in policies)
            {
                var authorized = await _authorization.AuthorizeAsync(context.HttpContext.User, policy);
                if (authorized.Succeeded) return;
            }

            context.Result = isAuthenticated ? new StatusCodeResult(403) : new UnauthorizedResult();
        }
    }
}