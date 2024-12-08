using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.IOC.IdentityServer4Configs;

public static class IdentityServer4Config
{
    public static object RoleIdentity { get; }

    public static IServiceCollection Add_SSOAPI_Config(this IServiceCollection services, string Client_Id,
        string Authority, string Audience)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.Audience = Audience; // "https://localhost:44305/resources";
            options.Authority = Authority; // "https://localhost:44305";

            // options.Events = new JwtBearerEvents
            // {
            //     OnTokenValidated = context =>
            //     {
            //         //Get the calling app client id that came from the token produced by Azure AD
            //         string NationalCodeUser = context.Principal.FindFirstValue("iat");
            //         UserManager<User> userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<User>>();
            //         ClaimsPrincipal userPrincipal = context.Principal;
            //         
            //         if (userPrincipal.HasClaim(c => c.Type == "name"))
            //         {
            //             string UserName = userPrincipal.Claims.First(c => c.Type == "name").Value;
            //         }
            //
            //         var FindUser = userManager.FindByNameAsync(NationalCodeUser).Result;
            //         var UserRole = userManager.GetRolesAsync(FindUser).Result;
            //         var UserClaim = userManager.GetClaimsAsync(FindUser).Result;
            //
            //         //if (!context.Principal.HasClaim(c => c.Value == Client_Id))
            //         //{
            //         //    context.Fail($"The claim client_id is not present in the token.");
            //         //}
            //
            //         //Add claim if they are
            //         var claims = new List<Claim>();
            //         
            //         foreach (var userrole in UserRole)
            //         {
            //             claims.Add(new Claim(ClaimTypes.Role, userrole));
            //         }
            //         var appIdentity = new ClaimsIdentity(claims);
            //         foreach (var userclaim in UserClaim)
            //         {
            //             claims.Add(new Claim(userclaim.Type, userclaim.Value));
            //         }
            //         appIdentity = new ClaimsIdentity(claims);
            //
            //         context.Principal.AddIdentity(appIdentity);
            //
            //         return Task.CompletedTask;
            //     }
            // };
        });
        return services;
    }
}