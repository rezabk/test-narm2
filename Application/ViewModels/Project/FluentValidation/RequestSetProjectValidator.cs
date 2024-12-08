using Common;
using FluentValidation;

namespace Application.ViewModels.Project.FluentValidation;

public class RequestSetProjectValidator : AbstractValidator<RequestSetProjectViewModel>
{
    public RequestSetProjectValidator()
    {
        
        RuleFor(x => x.ClassId)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("لطفا شناسه کلاس انتخاب کنید");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("لطفا عنوان سوال را وارد کنید");
        
        RuleFor(x => x.File)
            .NotEmpty()
            .NotNull()
            .WithMessage("لطفا فایل پروژه را بارگذاری کنید");


        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("تاریخ شروع تخفیف باید وارد شود")
            .Must(BeTodayOrLater)
            .WithMessage("تاریخ شروع تخفیف نمیتواند قبل از امروز باشد");

        RuleFor(x => x.EndDate)
            .Must(BeTodayOrLater)
            .WithMessage("تاریخ پایان تخفیف نمیتواند قبل از امروز باشد");

        RuleFor(x => x)
            .Custom((model, context) =>
            {
                if (!string.IsNullOrEmpty(model.EndDate))
                {
                    var startDate = model.StartDate.ConvertJalaliToMiladi();
                    var endDate = model.EndDate.ConvertJalaliToMiladi();

                    if (endDate < startDate)
                    {
                        context.AddFailure("تاریخ پایان تخفیف نمی‌تواند قبل از تاریخ شروع تخفیف باشد.");
                    }
                }
            });
    }


    private bool BeTodayOrLater(string startDate)
    {
        DateTime convertedDate = startDate.ConvertJalaliToMiladi();
        DateTime today = DateTime.Today;

        return convertedDate.Date >= today;
    }
}