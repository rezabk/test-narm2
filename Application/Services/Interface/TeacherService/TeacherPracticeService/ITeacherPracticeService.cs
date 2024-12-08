using Application.ViewModels.Practice;

namespace Application.Services.Interface.TeacherService.TeacherPracticeService;

public interface ITeacherPracticeService
{
    Task<List<ShowPracticeByClassId>> GetAllPracticeByClassId(int classId);
    
    Task<int> SetPractice(RequestSetPracticeViewModel model);

    Task<bool> RemovePractice(int practiceId);
}