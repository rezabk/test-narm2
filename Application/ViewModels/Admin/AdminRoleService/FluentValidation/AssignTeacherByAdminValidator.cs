using FluentValidation;

namespace Application.ViewModels.Admin.AdminRoleService.FluentValidation;

public class AssignTeacherByAdminValidator : AbstractValidator<RequestAssignTeacherByAdminViewModel>
{
    public AssignTeacherByAdminValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .GreaterThan(0).WithMessage("لطفا آیدی کاربر را وارد کنید");

        RuleFor(x => x.UniversityName)
            .NotEmpty().WithMessage("لطفا نام دانشگاه را وارد کنید");

        RuleFor(x => x.TeacherField)
            .IsInEnum().WithMessage("لطفا رشته را صحیح انتخاب کنید");
    }
}