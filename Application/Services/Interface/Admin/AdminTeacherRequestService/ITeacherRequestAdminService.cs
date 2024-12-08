using Application.ViewModels.Admin.AdminTeacherRequest;

namespace Application.Services.Interface.Admin.AdminTeacherRequestService;

public interface ITeacherRequestAdminService
{
    Task<List<ShowTeacherRequestForAdmin>> GetNewTeacherRequest();

    Task<bool> AcceptRequest(int requestId);

    Task<bool> RejectRequest(int requestId);
}