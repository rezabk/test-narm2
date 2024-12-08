using Application.Cross.Interface;
using Application.IRepositories;
using Application.Services.Interface.Admin.AdminRoleService;
using Application.Services.Interface.Logger;
using Application.Services.Interface.ProfileService.ProfileValidator;
using Application.ViewModels.Admin.AdminRoleService;
using Common.Enums;
using Common.Enums.RolesManagment;
using Common.ExceptionType.CustomException;
using Domain.Entities.TeacherEntities;
using Domain.Entities.UserAgg;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Services.Concrete.Admin.AdminRoleService;

public class AdminRoleService : IAdminRoleService
{
    private readonly IRepository<Teacher> _teacherRepository;

    private readonly ICustomLoggerService<AdminRoleService> _logger;
    private readonly UserManager<ApplicationUser> _userManager;


    public AdminRoleService(IUnitOfWork unitOfWork,
        ICustomLoggerService<AdminRoleService> logger,
        UserManager<ApplicationUser> userManager)
    {
        _teacherRepository = unitOfWork.GetRepository<Teacher>();
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<bool> AssignTeacher(RequestAssignTeacherByAdminViewModel model)
    {
        var user = await _userManager.Users.Where(x => x.Id == model.UserId).FirstOrDefaultAsync() ??
                   throw new NotFoundException();

        if (await _userManager.IsInRoleAsync(user, nameof(UserRolesEnum.Teacher)))
            throw new FormValidationException(MessageId.AlreadyTeacher);

        if (!string.IsNullOrWhiteSpace(user.FirstName) || !string.IsNullOrWhiteSpace(user.LastName))
            throw new FormValidationException(MessageId.CompleteProfile);

        var newTeacher = new Teacher
        {
            UserId = user.Id,
            Description = model.Description,
            TeacherField = model.TeacherField,
            UniversityName = model.UniversityName
        };

        try
        {
            await _teacherRepository.AddAsync(newTeacher);
            await _userManager.AddToRoleAsync(user, nameof(UserRolesEnum.Teacher));
            await _userManager.RemoveFromRoleAsync(user, nameof(UserRolesEnum.Student));

            return true;
        }
        catch (Exception excpetion)
        {
            await _teacherRepository.RemoveAsync(newTeacher, true);
            _logger.LogAddError(excpetion, "Teacher");
            throw excpetion ?? throw new ErrorException();
        }
    }
}