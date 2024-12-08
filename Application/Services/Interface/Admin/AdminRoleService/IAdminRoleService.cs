using Application.ViewModels.Admin.AdminRoleService;

namespace Application.Services.Interface.Admin.AdminRoleService;

public interface IAdminRoleService
{
    Task<bool> AssignTeacher( RequestAssignTeacherByAdminViewModel model);
}