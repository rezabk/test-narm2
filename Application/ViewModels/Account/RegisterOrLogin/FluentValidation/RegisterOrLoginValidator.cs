using FluentValidation;

namespace Application.ViewModels.Account.FluentValidation;

public class RegisterOrLoginValidator : AbstractValidator<RegisterOrLoginViewModel>
{
    public RegisterOrLoginValidator()
    {
        // Rule 1: Phone number must start with "09"
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("شماره موبایل را وارد کنید")
            .Must(p => p.StartsWith("09")).WithMessage("شماره موبایل باید با 09 شروع شود");

        // Rule 2: Phone number must be exactly 11 digits long
        RuleFor(x => x.PhoneNumber)
            .Length(11).WithMessage("شماره موبایل باید 11 رقم باشد");
    }
}