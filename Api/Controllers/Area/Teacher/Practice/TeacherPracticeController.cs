using Application.Services.Interface.TeacherService.TeacherPracticeService;
using Application.ViewModels.Practice;
using Common.Enums.RolesManagment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Area.Teacher.Practice;

[Area("Teacher")]
[Authorize(Roles = nameof(UserRolesEnum.Teacher))]
[Route("/api/teacher/practice")]
public class TeacherPracticeController: BaseController
{
    private readonly ITeacherPracticeService _teacherPracticeService;

    public TeacherPracticeController(ITeacherPracticeService teacherPracticeService)
    {
        _teacherPracticeService = teacherPracticeService;
    }

    [HttpGet("[action]")]
    public async Task<List<ShowPracticeByClassId>> GetAllPracticeByClassId(int classId)
    {
        return await _teacherPracticeService.GetAllPracticeByClassId(classId);
    }
    
    
    [HttpPost("[action]")]
    public async Task<int> SetPractice([FromBody] RequestSetPracticeViewModel model)
    {
        return await _teacherPracticeService.SetPractice(model);
    }
    
       
    [HttpDelete("[action]")]
    public async Task<bool> RemovePractice(int practiceId)
    {
        return await _teacherPracticeService.RemovePractice(practiceId);
    }
    
}