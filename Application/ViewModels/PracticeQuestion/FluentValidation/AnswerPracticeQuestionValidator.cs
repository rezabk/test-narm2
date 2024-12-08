using FluentValidation;

namespace Application.ViewModels.PracticeQuestion.FluentValidation;

public class AnswerPracticeQuestionValidator : AbstractValidator<RequestAnswerPracticeQuestionViewModel>
{
    public AnswerPracticeQuestionValidator()
    {
        RuleFor(x => x.Description).NotEmpty().NotNull().WithMessage("توضیحات نمیتواند خالی باشد");
    }
}