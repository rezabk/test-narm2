using Application.IRepositories;
using Application.Services.Base;
using Application.Services.Interface.Logger;
using Application.Services.Interface.TeacherRequestService;
using Application.ViewModels.Teacher;
using Application.ViewModels.TeacherRequest;
using Common.Enums;
using Common.Enums.TeacherEnum;
using Common.ExceptionType.CustomException;
using Domain.Entities.TeacherEntities;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Concrete.TeacherRequestService;

public class TeacherRequestService : ServiceBase<TeacherRequestService>, ITeacherRequestService
{
    private readonly IRepository<TeacherRequest> _teacherRequestRepository;
    private readonly ICustomLoggerService<TeacherRequestService> _logger;

    public TeacherRequestService(IUnitOfWork unitOfWork, ICustomLoggerService<TeacherRequestService> logger,
        IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _teacherRequestRepository = unitOfWork.GetRepository<TeacherRequest>();
        _logger = logger;
    }


    public Task<ResponseTeacherRequestViewModel> ResponseTeacherRequest()
    {
        var request = _teacherRequestRepository.DeferredWhere(x => x.UserId == CurrentUserId).FirstOrDefault() ??
                      throw new NotFoundException();


        return Task.FromResult(new ResponseTeacherRequestViewModel
        {
            Description = request.AdminDescription,
            Status = request.Status,
            StatusTitle = request.Status.GetEnumDescription()
        });
    }

    public Task<bool> RequestToBeTeacher(RequestBeTeacherViewModel model)
    {
        if (_teacherRequestRepository.Any(x => x.UserId == CurrentUserId && x.Status == TeacherRequestStatusEnum.New))
            throw new FormValidationException(MessageId.OpenRequest);

        var newRequest = new TeacherRequest
        {
            UserId = CurrentUserId,
            Description = model.Description,
            Field = model.Field,
            Status = TeacherRequestStatusEnum.New,
        };

        try
        {
            _teacherRequestRepository.AddAsync(newRequest);
            _logger.LogAddSuccess("TeacherRequest", newRequest.Id);
            return Task.FromResult(true);
        }
        catch (Exception exception)
        {
            _logger.LogAddError(exception, "TeacherRequest");
            throw exception ?? throw new ErrorException();
        }
    }
}