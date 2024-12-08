using Common.Enums.TeacherEnum;
using Microsoft.AspNetCore.Http;

namespace Application.ViewModels.Admin.AdminRoleService;

public class RequestAssignTeacherByAdminViewModel
{
    public int UserId { get; set; }

    public string? Description { get; set; }

    public string UniversityName { get; set; }

    public TeacherFieldEnum TeacherField { get; set; }

}