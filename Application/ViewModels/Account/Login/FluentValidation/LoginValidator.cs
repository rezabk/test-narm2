using FluentValidation;

namespace Application.ViewModels.Account.Login.FluentValidation;

public class LoginValidator : AbstractValidator<LoginViewModel>
{
    public LoginValidator()
    {
        RuleFor(x => x.StudentId).NotEmpty().WithMessage("لطفا شماره دانشجویی را وارد کنید");
        RuleFor(x => x.Password).NotEmpty().WithMessage("لطفا رمز عبور خود را وارد کنید");
    }
}