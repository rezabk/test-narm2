using Application.ViewModels.Practice;

namespace Application.Services.Interface.TeacherService.TeacherPracticeStudentAnswerService;

public interface ITeacherPracticeStudentAnswerService
{
    Task<ShowPracticeAnswer> GetAllPracticeAnswerByUserId(int practiceId, int userId);

    Task<List<UserAnsweredList>> GetAllUserAnswered(int practiceId);

    Task<bool> ScorePracticeQuestionAnswer(int practiceQuestionAnswerId, double score);
}