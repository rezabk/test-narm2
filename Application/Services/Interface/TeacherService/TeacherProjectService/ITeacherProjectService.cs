using Application.ViewModels.Project;
using Application.ViewModels.Public;

namespace Application.Services.Interface.TeacherService.TeacherProjectService;

public interface ITeacherProjectService
{
    Task<List<ShowProjectViewModel>> GetAllProjectByClassId(int classId);
    Task<int> SetProject( RequestSetProjectViewModel model);

    Task<bool> RemoveProject(int projectId);
    
    Task<ResponseGetFileViewModel> GetProjectFile(string fileName);
}