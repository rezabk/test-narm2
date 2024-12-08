using Application.Services.Interface.AccountService;
using Application.ViewModels.Account;
using Application.ViewModels.Account.Login;
using Application.ViewModels.Account.RegisterWithStudentId;
using Application.ViewModels.Account.VerifyCode;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class AccountController : BaseController
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("[action]")]
    public async Task<int> Register([FromBody] RegisterViewModel model)
    {
        return await _accountService.Register(model);
    }

    [HttpPost("[action]")]
    public async Task<string> Login([FromBody] LoginViewModel model)
    {
        return await _accountService.Login(model);
    }
    

    [HttpPost("[action]")]
    public async Task<bool> Logout()
    {
        return await _accountService.Logout();
    }

    #region Legacy

    // [HttpPost("[action]")]
    // public async Task<int> RegisterOrLogin([FromBody] RegisterOrLoginViewModel model)
    // {
    //     return await _accountService.RegisterOrLogin(model);
    // }
    //
    //
    // [HttpPost("[action]")]
    // public async Task<int> ResendCode([FromBody] RegisterOrLoginViewModel model)
    // {
    //     return await _accountService.ResendCode(model);
    // }
    //
    // [HttpPost("[action]")]
    // public async Task<string> Login([FromBody] LoginViewModel model)
    // {
    //     return await _accountService.Login(model);
    // }
    //
    // [HttpPost("[action]")]
    // public async Task<string> VerifyCode([FromBody] VerifyCodeViewModel model)
    // {
    //     return await _accountService.VerifyCode(model);
    // }

    #endregion
    
 
}