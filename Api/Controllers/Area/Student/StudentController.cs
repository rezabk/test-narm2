using Application.Services.Interface.StudentService;
using Application.ViewModels.Class;
using Application.ViewModels.Public;
using Application.ViewModels.Teacher;
using Common.Enums.RolesManagment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Area.Student;

[Area("Student")]
[Authorize(Roles = nameof(UserRolesEnum.Student))]
[Route("/api/student/")]
public class StudentController : BaseController
{
    private readonly IStudentService _studentService;

    public StudentController(IStudentService studentService)
    {
        _studentService = studentService;
    }


    [HttpGet("[action]")]
    public async Task<List<ShowTeacherForUser>> GetAllTeacher()
    {
        return await _studentService.GetAllTeacher();
    }

    [HttpGet("[action]")]
    public async Task<List<ShowClassForUser>> GetAllClassByTeacherId(int teacherId)
    {
        return await _studentService.GetAllClassByTeacherId(teacherId);
    }

    [HttpPost("[action]")]
    public async Task<bool> JoinClass(int classId)
    {
        return await _studentService.JoinClass(classId);
    }
}