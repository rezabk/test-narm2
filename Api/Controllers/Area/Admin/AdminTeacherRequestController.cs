using Application.Services.Interface.Admin.AdminTeacherRequestService;
using Application.ViewModels.Admin.AdminTeacherRequest;
using Common.Enums.RolesManagment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Area.Admin;
[Area("Admin")]
[Authorize(Roles = nameof(UserRolesEnum.Admin))]
[Route("/api/admin/teacherrequest")]
public class AdminTeacherRequestController
{
    private readonly ITeacherRequestAdminService _teacherRequestAdminService;

    public AdminTeacherRequestController(ITeacherRequestAdminService teacherRequestAdminService)
    {
        _teacherRequestAdminService = teacherRequestAdminService;
    }
    
        
    [HttpGet("[action]")]
    public async Task<List<ShowTeacherRequestForAdmin>> GetNewTeacherRequest( )
    {
        return await _teacherRequestAdminService.GetNewTeacherRequest();
    }
    
    
           
    [HttpPost("[action]")]
    public async Task<bool> AcceptRequest(int requestId )
    {
        return await _teacherRequestAdminService.AcceptRequest(requestId);
    }
    
    [HttpPost("[action]")]
    public async Task<bool> RejectRequest(int requestId )
    {
        return await _teacherRequestAdminService.RejectRequest(requestId);
    }
}