using Application.Services.Interface.StudentPracticeService;
using Application.ViewModels.Practice;
using Application.ViewModels.PracticeQuestion;
using Common.Enums.RolesManagment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Area.Student.Practice;

[Area("Student")]
[Authorize(Roles = nameof(UserRolesEnum.Student))]
[Route("/api/student/practice")]
public class StudentPracticeController : BaseController
{
    private readonly IStudentPracticeService _studentPracticeService;

    public StudentPracticeController(IStudentPracticeService studentPracticeService)
    {
        _studentPracticeService = studentPracticeService;
    }

    [HttpGet("[action]")]
    public async Task<List<ShowPracticeByClassId>> GetAllPracticeByClassId(int classId)
    {
        return await _studentPracticeService.GetAllPracticeByClassId(classId);
    }

    [HttpGet("[action]")]
    public async Task<List<ShowPracticeQuestionViewModel>> GetAllPracticeQuestionByPracticeId(int practiceId)
    {
        return await _studentPracticeService.GetAllPracticeQuestionByPracticeId(practiceId);
    }

    [HttpPost("[action]")]
    public async Task<bool> AnswerPracticeQuestion([FromForm] RequestAnswerPracticeQuestionViewModel model)
    {
        return await _studentPracticeService.AnswerPracticeQuestion(model);
    }

    [HttpGet("[action]")]
    public Task<IResult> ServeQuestionImage(string fileName)
    {
        var response = _studentPracticeService.GetQuestionImage(fileName).Result;
        
        return Task.FromResult(Results.File(response.MemoryStream.ToArray(), "application/octet-stream",
            Path.GetFileName(response.FileName)));
    }
}