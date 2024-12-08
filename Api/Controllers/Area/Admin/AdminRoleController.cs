using Application.Services.Interface.Admin.AdminRoleService;
using Application.ViewModels.Admin.AdminRoleService;
using Common.Enums.RolesManagment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Area.Admin;

[Area("Admin")]
[Authorize(Roles = nameof(UserRolesEnum.Admin))]
[Route("/api/admin/roles")]
public class AdminRoleController : BaseController
{
    private readonly IAdminRoleService _adminRoleService;

    public AdminRoleController(IAdminRoleService adminRoleService)
    {
        _adminRoleService = adminRoleService;
    }
    
    [HttpPost("[action]")]
    public async Task<bool> AssignTeacher([FromBody] RequestAssignTeacherByAdminViewModel model)
    {
        return await _adminRoleService.AssignTeacher(model);
    }
}