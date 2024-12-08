using Microsoft.AspNetCore.Mvc;

namespace Stap.Api.Helper.Auth;

public class AuthorizeMultiplePolicyAttribute : TypeFilterAttribute
{
    public AuthorizeMultiplePolicyAttribute(string policy, bool IsAll) : base(typeof(AuthorizeMultiplePolicyFilter))
    {
        Arguments = new object[] { policy, IsAll };
    }
}