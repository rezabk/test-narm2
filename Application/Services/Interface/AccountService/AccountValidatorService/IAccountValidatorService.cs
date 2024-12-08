using Application.ViewModels.Account.VerifyCode;
using StapSSO.Enums;

namespace Application.Services.Interface.AccountService.AccountValidatorService;

public interface IAccountValidatorService
{
    Task ValidateCode(VerifyCodeViewModel model);

    Task<AuthEnum> CheckForAuthAction(string phoneNumber);
}