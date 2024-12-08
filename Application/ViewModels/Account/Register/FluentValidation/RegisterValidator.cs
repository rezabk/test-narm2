using FluentValidation;

namespace Application.ViewModels.Account.RegisterWithStudentId.FluentValidation;

public class RegisterValidator : AbstractValidator<RegisterViewModel>
{
    public RegisterValidator()
    {
        RuleFor(x => x.StudentId)
            .NotEmpty()
            .WithMessage("لطفا شماره دانشجویی خود را وارد کنید");

        RuleFor(x => x.RePassword)
            .Equal(x => x.Password)
            .WithMessage("رمزعبور و تکرار آن باید یکسان باشند");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("رمزعبور را وارد کنید")
            .MinimumLength(8)
            .WithMessage("رمز عبور باید حداقل 8 کاراکتر و شامل حداقل یک حرف کوچک، یک حرف بزرگ و حداقل یک عدد باشد")
            .Matches(@"[A-Z]")
            .WithMessage("رمز عبور باید حداقل 8 کاراکتر و شامل حداقل یک حرف کوچک، یک حرف بزرگ و حداقل یک عدد باشد")
            .Matches(@"[a-z]")
            .WithMessage("رمز عبور باید حداقل 8 کاراکتر و شامل حداقل یک حرف کوچک، یک حرف بزرگ و حداقل یک عدد باشد")
            .Matches(@"\d")
            .WithMessage("رمز عبور باید حداقل 8 کاراکتر و شامل حداقل یک حرف کوچک، یک حرف بزرگ و حداقل یک عدد باشد");
    }
}