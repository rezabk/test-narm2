using Application.ViewModels.Account;
using Application.ViewModels.Account.Login;
using Application.ViewModels.Account.RegisterWithStudentId;
using Application.ViewModels.Account.VerifyCode;

namespace Application.Services.Interface.AccountService;

public interface IAccountService
{
  
    Task<int> Register(RegisterViewModel model);

    Task<string> Login(LoginViewModel model);
    
    Task<bool> Logout();

    #region Legacy Authentication

    // Task<int> RegisterOrLogin(RegisterOrLoginViewModel model);
    //
    //
    // Task<int> ResendCode(RegisterOrLoginViewModel model);
    //
    // Task<string> Login(LoginViewModel model);
    //
    // Task<string> VerifyCode(VerifyCodeViewModel model);

        #endregion
  

    
}