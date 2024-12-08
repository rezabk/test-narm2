using Application.Services.Interface.TeacherRequestService;
using Application.ViewModels.Teacher;
using Application.ViewModels.TeacherRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
public class TeacherRequestController  :BaseController
{
    private readonly ITeacherRequestService _teacherRequestService;

    public TeacherRequestController(ITeacherRequestService teacherRequestService)
    {
        _teacherRequestService = teacherRequestService;
    }
    
    
    [HttpGet("[action]")]
    public async Task<ResponseTeacherRequestViewModel> ResponseTeacherRequest( )
    {
        return await _teacherRequestService.ResponseTeacherRequest();
    }

      
    [HttpPost("[action]")]
    public async Task<bool> RequestToBeTeacher(RequestBeTeacherViewModel model)
    {
        return await _teacherRequestService.RequestToBeTeacher(model);
    }

}