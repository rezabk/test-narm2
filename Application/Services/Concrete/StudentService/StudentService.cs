using Application.IRepositories;
using Application.Services.Base;
using Application.Services.Interface.Logger;
using Application.Services.Interface.StudentService;
using Application.ViewModels.Class;
using Application.ViewModels.Public;
using Application.ViewModels.Teacher;
using Common;
using Common.Enums;
using Common.Enums.RolesManagment;
using Common.ExceptionType.CustomException;
using Domain.Entities.ClassEntities;
using Domain.Entities.TeacherEntities;
using Domain.Entities.UserAgg;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services.Concrete.StudentService;

public class StudentService : ServiceBase<StudentService>, IStudentService
{
    private readonly IRepository<Teacher> _teacherRepository;
    private readonly IRepository<Class> _classRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICustomLoggerService<StudentService> _logger;

    public StudentService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager,
        ICustomLoggerService<StudentService> logger
        , IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _teacherRepository = unitOfWork.GetRepository<Teacher>();
        _classRepository = unitOfWork.GetRepository<Class>();
        _userManager = userManager;
        _logger = logger;
    }


    public Task<List<ShowTeacherForUser>> GetAllTeacher()
    {
        var userTeacher = _userManager.GetUsersInRoleAsync(nameof(UserRolesEnum.Teacher)).Result;
        var teachers = _teacherRepository.DeferredWhere(x => userTeacher.Select(u => u.Id).Contains(x.UserId))
            .Include(x => x.User);

        return Task.FromResult(teachers.Select(x => new ShowTeacherForUser
        {
            Id = x.Id,
            FirstName = x.User.FirstName ?? "",
            LastName = x.User.LastName ?? "",
            TeacherField = x.TeacherField,
            TeacherFieldTitle = x.TeacherField.GetEnumDescription(),
            TotalClasses = x.Classes.Count(),
        }).ToList());
    }

    public Task<List<ShowClassForUser>> GetAllClassByTeacherId(int teacherId)
    {
        var classes = _classRepository.DeferredWhere(x => x.Teacher != null && x.TeacherId == teacherId)
            .Include(x => x.Students);

        return Task.FromResult(classes.Select(x => new ShowClassForUser
        {
            Id = x.Id,
            Description = x.Description,
            UniversityName = x.UniversityName,
            Semester = x.Semester,
            SemesterTitle = x.Semester.GetEnumDescription(),
            CreationDate = x.InsertTime.ConvertMiladiToJalali(),
            TotalAllowedStudent = x.TotalAllowedStudent,
            CurrentStudents = x.Students.Count
        }).ToList());
    }

    public Task<bool> JoinClass(int classId)
    {
        var classRoom = _classRepository.DeferredWhere(x => x.Teacher != null && x.Id == classId)
                            .Include(x => x.Students)
                            .FirstOrDefault() ??
                        throw new NotFoundException();
        var student = _userManager.Users.FirstOrDefault(x => x.Id == CurrentUserId);

        if (classRoom.TotalAllowedStudent <= classRoom.Students.Count)
            throw new FormValidationException(MessageId.ClassCapacityLimit);

        if (classRoom.Students.Select(x=>x.Id).Contains(student.Id)) throw new FormValidationException(MessageId.AlreadyInClass);

        try
        {
            classRoom.Students.Add(student);
            _classRepository.UpdateAsync(classRoom, true);
            _logger.LogInformation($"Student with Id {student.Id} joined class with id {classRoom.Id}");
            return Task.FromResult(true);
        }
        catch (Exception exception)
        {
            _logger.LogError($"Student with Id {student.Id} Failed to join class with id {classRoom.Id}");
            throw exception ?? throw new ErrorException();
        }
    }
}