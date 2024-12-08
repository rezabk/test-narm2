using FluentValidation;

namespace Application.ViewModels.PracticeQuestion.FluentValidation;

public class SetQuestionValidator : AbstractValidator<RequestSetQuestionViewModel>
{
    public SetQuestionValidator()
    {
        RuleFor(x => x.PracticeId).NotNull().GreaterThan(0).WithMessage("لطفا شناسه تمرین را وارد کنید");

        RuleFor(x => x.Title).NotEmpty().NotNull().WithMessage("لطفا عنوان سوال را وارد کنید");
    }
}