using Application.ViewModels.Class;
using Application.ViewModels.Public;

namespace Application.Services.Interface.TeacherService.TeacherClassService;

public interface ITeacherClassService
{
    Task<List<SelectOptionViewModel>> GetAllClass();
    Task<ResponseGetAllClassByFilterViewModel> GetAllClassByFilter(RequestGetAllClassByFilterViewModel model);
    Task<int> SetClass(RequestSetClassViewModel model);
    Task<bool> RemoveClass(int classId);
}