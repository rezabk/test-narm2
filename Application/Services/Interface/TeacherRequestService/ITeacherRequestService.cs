using Application.ViewModels.Teacher;
using Application.ViewModels.TeacherRequest;

namespace Application.Services.Interface.TeacherRequestService;

public interface ITeacherRequestService
{

    Task<ResponseTeacherRequestViewModel> ResponseTeacherRequest();
    Task<bool> RequestToBeTeacher(RequestBeTeacherViewModel model);
}