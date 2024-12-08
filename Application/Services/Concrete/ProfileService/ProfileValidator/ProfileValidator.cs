using System.ComponentModel.DataAnnotations;
using Application.DTO;
using Application.IRepositories;
using Application.Services.Interface.ProfileService.ProfileValidator;
using Application.ViewModels.Account;
using Application.ViewModels.Profile;
using Common.Enums;
using Common.ExceptionType.CustomException;
using Domain.Entities.UserAgg;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Concrete.ProfileService.ProfileValidator;

public class ProfileValidator : IProfileValidator
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfileValidator(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> ValidateUpdateUser(ApplicationUser user, UpdateUserViewModel model)
    {
        if (await _userManager.Users.Where(x => x.Id != user.Id)
                .AnyAsync(x => x.Email.ToLower() == model.Email.ToLower()))
            throw new FormValidationException(MessageId.EmailExits);
        
        if (await _userManager.Users.Where(x => x.Id != user.Id)
                .AnyAsync(x => x.PhoneNumber.ToLower() == model.PhoneNumber.ToLower()))
            throw new FormValidationException(MessageId.PhoneNumberExits);


        if (!IsValidEmail(model.Email)) throw new FormValidationException(MessageId.WrongEmail);
        
        


        return true;
    }

    private bool IsValidEmail(string email)
    {
        var emailAttribute = new EmailAddressAttribute();
        return emailAttribute.IsValid(email);
    }
}