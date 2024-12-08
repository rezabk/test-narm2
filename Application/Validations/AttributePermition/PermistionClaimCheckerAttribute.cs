using System.ComponentModel.DataAnnotations;
using Common.GetClaimUtils;
using Microsoft.AspNetCore.Http;

namespace Application.Validations.AttributePermition;

public class PermistionClaimCheckerAttribute : ValidationAttribute
{
    //public object _ClaimValue { get; set; }
    public PermistionClaimCheckerAttribute(string claimtype) //, object claimvalue)
    {
        _ClaimType = claimtype;
        //_ClaimValue = claimvalue;
    }

    public string _ClaimType { get; set; }

    public string GetErrorMessage()
    {
        return "شما داده\u200cای خارج از محدوده دسترسی خود انتخاب کرده\u200cاید";
    }

    protected override ValidationResult IsValid(
        object value,
        ValidationContext validationContext)
    {
        var httpContextAccessor = (IHttpContextAccessor)validationContext.GetService(typeof(IHttpContextAccessor));
        var user = httpContextAccessor.HttpContext.User;

        var _claimvalues = user.GetUserClaimsValue(_ClaimType);

        var PropertyValue = (int)value!;

        if (!_claimvalues.Contains(PropertyValue.ToString())) return new ValidationResult(GetErrorMessage());

        return ValidationResult.Success;
    }
}