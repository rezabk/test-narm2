using Application.DTO;
using Application.IRepositories;
using Application.Services.Interface.AccountService.AccountValidatorService;
using Application.ViewModels.Account.VerifyCode;
using Common.Enums;
using Common.ExceptionType.CustomException;
using Domain.Entities;
using Domain.Entities.UserAgg;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StapSSO.Enums;

namespace Application.Services.Concrete.AccountService.ValidateCodeService;

public class AccountValidatorService : IAccountValidatorService
{
    private readonly IRepository<PhoneNumberCode> _phoneNumberCodeRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountValidatorService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)

    {
        _userManager = userManager;
        _phoneNumberCodeRepository = unitOfWork.GetRepository<PhoneNumberCode>();
    }


    public async Task ValidateCode(VerifyCodeViewModel model)
    {
        if (!_phoneNumberCodeRepository.Any(x => x.PhoneNumber == model.PhoneNumber))
            throw new FormValidationException(MessageId.MobileNotFound);

        if (!_phoneNumberCodeRepository.Any(x => x.GeneratedCode == model.Code))
            throw new FormValidationException(MessageId.FalseCode);

        if (_phoneNumberCodeRepository.Any(x =>
                x.PhoneNumber == model.PhoneNumber && x.GeneratedCode == model.Code && x.IsUsed == true))
            throw new FormValidationException(MessageId.CodeUsed);

        var entity = await _phoneNumberCodeRepository.FirstOrDefaultItemAsync(x =>
            x.PhoneNumber == model.PhoneNumber && x.GeneratedCode == model.Code && x.IsUsed == false);
        if (entity.ExpireDate < DateTime.UtcNow)
            throw new FormValidationException(MessageId.CodeExpired);

        await _phoneNumberCodeRepository.UpdateAsync(entity, entity.IsUsed = true);
    }

    public async Task<AuthEnum> CheckForAuthAction(string phoneNumber)
    {
        if (await _userManager.Users.AnyAsync(x => x.PhoneNumber == phoneNumber)) return AuthEnum.Login;

        return AuthEnum.Register;
    }
}