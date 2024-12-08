using Application.Services.Interface.TeacherService.TeacherPracticeStudentAnswerService;
using Application.ViewModels.Practice;
using Common.Enums.RolesManagment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Area.Teacher.TeacherPracticeStudentAnswer;

[Area("Teacher")]
[Authorize(Roles = nameof(UserRolesEnum.Teacher))]
[Route("/api/teacher/practicequestionstudentanswer")]
public class TeacherPracticeStudentAnswerController : BaseController
{
    private readonly ITeacherPracticeStudentAnswerService _teacherPracticeStudentAnswerService;

    public TeacherPracticeStudentAnswerController(
        ITeacherPracticeStudentAnswerService teacherPracticeStudentAnswerService)
    {
        _teacherPracticeStudentAnswerService = teacherPracticeStudentAnswerService;
    }

    [HttpGet("[action]")]
    public async Task<List<UserAnsweredList>> GetAllUserAnswered(int practiceId)
    {
        return await _teacherPracticeStudentAnswerService.GetAllUserAnswered(practiceId);
    }

    [HttpGet("[action]")]
    public async Task<ShowPracticeAnswer> GetAllPracticeAnswerByUserId(int practiceId, int userId)
    {
        return await _teacherPracticeStudentAnswerService.GetAllPracticeAnswerByUserId(practiceId, userId);
    }

    [HttpPost("[action]")]
    public async Task<bool> ScorePracticeQuestionAnswer(int practiceQuestionAnswerId, double score)
    {
        return await _teacherPracticeStudentAnswerService.ScorePracticeQuestionAnswer(practiceQuestionAnswerId, score);
    }
}