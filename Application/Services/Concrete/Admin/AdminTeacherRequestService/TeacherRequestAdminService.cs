using Application.IRepositories;
using Application.Services.Base;
using Application.Services.Interface.Admin.AdminTeacherRequestService;
using Application.Services.Interface.Logger;
using Application.ViewModels.Admin.AdminTeacherRequest;
using Common;
using Common.Enums;
using Common.Enums.TeacherEnum;
using Common.ExceptionType.CustomException;
using Domain.Entities.TeacherEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Concrete.Admin.AdminTeacherRequestService;

public class TeacherRequestAdminService : ServiceBase<TeacherRequestAdminService>, ITeacherRequestAdminService
{
    private readonly IRepository<TeacherRequest> _teacherRequestService;
    private readonly ICustomLoggerService<TeacherRequestAdminService> _logger;

    public TeacherRequestAdminService(IUnitOfWork unitOfWork,
        ICustomLoggerService<TeacherRequestAdminService> logger,
        IHttpContextAccessor contextAccessor) : base(contextAccessor)
    {
        _teacherRequestService = unitOfWork.GetRepository<TeacherRequest>();
        _logger = logger;
    }

    public Task<List<ShowTeacherRequestForAdmin>> GetNewTeacherRequest()
    {
        var request = _teacherRequestService.DeferredWhere(x => x.Status == TeacherRequestStatusEnum.New)
            .Include(x => x.User);

        return Task.FromResult(request.Select(x => new ShowTeacherRequestForAdmin
        {
            Id = x.Id,
            Description = x.Description,
            Field = x.Field,
            Status = x.Status,
            UserId = x.UserId,
            FullName = x.User.FirstName + " " + x.User.LastName,
            FieldTitle = x.Field.GetEnumDescription(),
            StatusTitle = x.Status.GetEnumDescription(),
            RequestTime = x.InsertTime.ConvertMiladiToJalali()
        }).ToList());
    }

    public Task<bool> AcceptRequest(int requestId)
    {
        var request =
            _teacherRequestService.DeferredWhere(x => x.Status == TeacherRequestStatusEnum.New && x.Id == requestId)
                .FirstOrDefault() ?? throw new NotFoundException();

        request.Status = TeacherRequestStatusEnum.Approved;

        try
        {
            _teacherRequestService.UpdateAsync(request, true);
            _logger.LogUpdateSuccess("TeacherRequest", request.Id);
            return Task.FromResult(true);
        }
        catch (Exception exception)
        {
            _logger.LogUpdateError(exception, "TeacherRequest", request.Id);
            throw exception ?? throw new ErrorException();
        }
    }

    public Task<bool> RejectRequest(int requestId)
    {
        var request =
            _teacherRequestService.DeferredWhere(x => x.Status == TeacherRequestStatusEnum.New && x.Id == requestId)
                .FirstOrDefault() ?? throw new NotFoundException();

        request.Status = TeacherRequestStatusEnum.Rejected;

        try
        {
            _teacherRequestService.UpdateAsync(request, true);
            _logger.LogUpdateSuccess("TeacherRequest", request.Id);
            return Task.FromResult(true);
        }
        catch (Exception exception)
        {
            _logger.LogUpdateError(exception, "TeacherRequest", request.Id);
            throw exception ?? throw new ErrorException();
        }
    }
}