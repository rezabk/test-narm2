using FluentValidation;

namespace Application.ViewModels.Class.FluentValidation;

public class SetClassViewModelValidator : AbstractValidator<RequestSetClassViewModel>
{
    public SetClassViewModelValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("لطفا عنوان کلاس را وارد کنید");

        RuleFor(x => x.Semester)
            .IsInEnum().WithMessage("لطفا ترم را صحیح انتخاب کنید");

        RuleFor(x => x.TotalAllowedStudent)
            .GreaterThan(0).WithMessage("تعداد دانشجویان مجاز به ورود باید بیشتر از 0 باشد");
    }
}