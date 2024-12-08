using System.Security.Claims;
using Application.Cross.Interface;
using Application.IRepositories;
using Application.Services.Base;
using Application.Services.Interface.AccountService;
using Application.Services.Interface.AccountService.AccountValidatorService;
using Application.Services.Interface.Logger;
using Application.Services.Interface.Sms;
using Application.Services.Interface.Utils;
using Application.ViewModels.Account;
using Application.ViewModels.Account.Login;
using Application.ViewModels.Account.RegisterWithStudentId;
using Application.ViewModels.Account.VerifyCode;
using Common.Enums;
using Common.Enums.RolesManagment;
using Common.ExceptionType.CustomException;
using Domain.Entities;
using Domain.Entities.UserAgg;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StapSSO.Enums;

namespace Application.Services.Concrete.AccountService;

public class AccountService : ServiceBase<AccountService>, IAccountService
{
    private readonly IRepository<PhoneNumberCode> _phoneNumberCodeRepository;
    private readonly ICustomLoggerService<AccountService> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ISmsService _smsService;
    private readonly ITokenService _tokenService;
    private readonly ILiaraUploader _uploader;
    private readonly IAccountValidatorService _accountValidatorService;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public AccountService(IHttpContextAccessor contextAccessor, IUnitOfWork unitOfWork,
        ICustomLoggerService<AccountService> logger,
        ISmsService smsService, ITokenService tokenService, ILiaraUploader uploader,
        IAccountValidatorService accountValidatorService,
        IConfiguration configuration, IWebHostEnvironment hostingEnvironment,
        UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : base(contextAccessor)
    {
        _phoneNumberCodeRepository = unitOfWork.GetRepository<PhoneNumberCode>();
        _userManager = userManager;
        _logger = logger;
        _smsService = smsService;
        _tokenService = tokenService;
        _uploader = uploader;
        _accountValidatorService = accountValidatorService;
        _configuration = configuration;
        _hostingEnvironment = hostingEnvironment;
        _signInManager = signInManager;
    }


    public async Task<int> Register(RegisterViewModel model)
    {
        if (_userManager.Users.Any(x => x.UserName == model.StudentId || x.StudentId == model.StudentId))
            throw new FormValidationException(MessageId.UserExists);
        
        var newUser = new ApplicationUser
        {
            UserName = model.StudentId,
            StudentId = model.StudentId
        };


        try
        {
            await _userManager.CreateAsync(newUser);
            await _userManager.AddPasswordAsync(newUser, model.Password);
            await _userManager.AddToRoleAsync(newUser, nameof(UserRolesEnum.Student));
            var claims = new List<Claim>
            {
                new("user_id", newUser.Id.ToString()),
                new("user_studentId", newUser.StudentId)
            };
            await _userManager.AddClaimsAsync(newUser, claims);

            _logger.LogAddSuccess("User", newUser.Id);
            return newUser.Id;
        }
        catch (Exception exception)
        {
              await _userManager.DeleteAsync(newUser);
            _logger.LogAddError(exception, "User");
            throw new ErrorException();
        }
    }

    public Task<string> Login(LoginViewModel model)
    {
        var user = _userManager.Users.Where(x => x.UserName == model.StudentId).FirstOrDefault() ??
                   throw new NotFoundException();

        var resultLogin = _signInManager.PasswordSignInAsync(user, model.Password, false, false).Result;
        if (!resultLogin.Succeeded) throw new FormValidationException(MessageId.WrongLoginInfo);

        var token = _tokenService.CreateToken(user).Result;

        return Task.FromResult(token);
    }


    public async Task<bool> Logout()
    {
        try
        {
            await _signInManager.SignOutAsync();
            return true;
        }
        catch (Exception e)
        {
            throw e ?? throw new ErrorException();
        }
    }

    ////////////////////
    ////////////////////  
    // Code below use this logic
    // RegisterOrLogin => user inputs phone number and a code will be sent to phonenumber
    // VerifyCode => user enters code and will be logged in or registered ,, Username will be phonenumber
    ////////////////////  
    ////////////////////

    #region Legacy Authentication

    // public async Task<int> RegisterOrLogin(RegisterOrLoginViewModel model)
    // {
    //     var generateVerifyCode = await GenerateVerifyCode(model.PhoneNumber);
    //
    //     var canSendSmsCode = _configuration.GetSection("Sms:SendSmsCode").Value == "1";
    //     if (canSendSmsCode)
    //     {
    //         // var result = await SendSmsForRegisterOrLogin(model.PhoneNumber, generateVerifyCode,
    //         //     SmsPatternEnum.VerifyCode);
    //         // if (!result.Success)
    //         // {
    //         //     throw result.Exception;
    //         // }
    //         await _smsService.SendSmsBasicVersion(model.PhoneNumber, generateVerifyCode);
    //     }
    //
    //     return generateVerifyCode;
    // }

    // public async Task<int> ResendCode(RegisterOrLoginViewModel model)
    // {
    //     var generateVerifyCode = await GenerateVerifyCode(model.PhoneNumber);
    //
    //
    //     var canSendSmsCode = _configuration.GetSection("Sms:SendSmsCode").Value == "1";
    //     if (canSendSmsCode)
    //     {
    //         // var result = await SendSmsForRegisterOrLogin(model.PhoneNumber, generateVerifyCode,
    //         //     SmsPatternEnum.VerifyCode);
    //         // if (result.Success)
    //         // {
    //         //     _logger.LogInformation($"Verify code resend to '{model.PhoneNumber}'");
    //         //     return generateVerifyCode;
    //         // }
    //         await _smsService.SendSmsBasicVersion(model.PhoneNumber, generateVerifyCode);
    //     }
    //
    //     return generateVerifyCode;
    // }
    //
    // public async Task<string> Login(LoginViewModel model)
    // {
    //     var user = await _userManager.Users.Where(x => x.UserName == model.PhoneNumber).FirstOrDefaultAsync() ??
    //                throw new NotFoundException();
    //
    //     var resultLogin = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
    //     if (!resultLogin.Succeeded) throw new ErrorException();
    //
    //     var token = await _tokenService.CreateToken(user);
    //     _logger.LogInformation($"user with id {user.Id} has logged in");
    //     return token;
    // }
    //
    //
    // public async Task<string> VerifyCode(VerifyCodeViewModel model)
    // {
    //     #region VALIDATE VERIFY CODE
    //
    //     await _accountValidatorService.ValidateCode(model);
    //
    //     #endregion
    //
    //     var checkForAuthAction = await _accountValidatorService.CheckForAuthAction(model.PhoneNumber);
    //
    //     #region IF USER AVAIABLE AND LOGIN
    //
    //     if (checkForAuthAction == AuthEnum.Login)
    //     {
    //         var token = await LoginWithPhoneNumber(model.PhoneNumber);
    //         _logger.LogInformation($"User with phoneNumber : '{model.PhoneNumber}' Logged in");
    //
    //         return token;
    //     }
    //
    //     #endregion
    //
    //     #region IF USER NEEDS TO BE REGISTERED
    //
    //     if (checkForAuthAction == AuthEnum.Register)
    //     {
    //         var user = new ApplicationUser
    //         {
    //             UserName = model.PhoneNumber,
    //             PhoneNumber = model.PhoneNumber
    //         };
    //
    //         var result = await _userManager.CreateAsync(user);
    //
    //         if (result.Succeeded)
    //         {
    //             var claims = new List<Claim>
    //             {
    //                 new("user_id", user.Id.ToString()),
    //                 new("user_phone_number", user.PhoneNumber)
    //             };
    //
    //             var resultAddClaim = await _userManager.AddClaimsAsync(user, claims);
    //             if (resultAddClaim.Succeeded)
    //                 _logger.LogInformation($"User with phoneNumber : '{model.PhoneNumber}' Logged in");
    //
    //
    //             var token = await LoginWithPhoneNumber(model.PhoneNumber);
    //
    //
    //             return token;
    //         }
    //     }
    //
    //     #endregion
    //
    //     throw new ErrorException();
    // }


    //////////
    ///   PRIVATE METHODS
    //////////
    // private async Task<string> LoginWithPhoneNumber(string phoneNumber)
    // {
    //     var user = await _userManager.Users.Where(x => x.PhoneNumber == phoneNumber).FirstOrDefaultAsync() ??
    //                throw new NotFoundException();
    //
    //     await _signInManager.SignInAsync(user, false);
    //
    //     var token = await _tokenService.CreateToken(user);
    //     _logger.LogInformation($"Token created for user with userId : '{user.Id}'");
    //
    //     return token;
    // }

    // private async Task<int> GenerateVerifyCode(string phoneNumber)
    // {
    //     var entity = new PhoneNumberCode
    //     {
    //         PhoneNumber = phoneNumber,
    //         CreatedDate = DateTime.Now,
    //         ExpireDate = DateTime.Now.AddMinutes(5),
    //         GeneratedCode = await GenerateCode(),
    //         IsUsed = false
    //     };
    //     try
    //     {
    //         await _phoneNumberCodeRepository.AddAsync(entity);
    //         _logger.LogAddSuccess("PhoneNumberCode", entity.Id);
    //         return entity.GeneratedCode;
    //     }
    //     catch (Exception e)
    //     {
    //         _logger.LogAddError(e, "PhoneNumberCode");
    //         throw e ?? throw new ErrorException();
    //     }
    // }
    //
    // private Task<int> GenerateCode()
    // {
    //     var random = new Random();
    //     var randomCode = random.Next(10000, 100000);
    //
    //     return Task.FromResult(randomCode);
    // }

    ///////
    //// USE WHEN YOU HAVE PRO KAVENEGAR SERVICE 
    //////
    // private async Task<ResponseSmsViewModel> SendSmsForRegisterOrLogin(string phoneNumber, int code,
    //     SmsPatternEnum pattern)
    // {
    //     var result = await _smsService.SendSms(phoneNumber, code, pattern);
    //     return result;
    // }

    #endregion
}