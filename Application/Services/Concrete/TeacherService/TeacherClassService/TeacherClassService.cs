using Application.IRepositories;
using Application.Services.Base;
using Application.Services.Interface.Logger;
using Application.Services.Interface.TeacherService.TeacherClassService;
using Application.ViewModels.Class;
using Application.ViewModels.Public;
using Common;
using Common.Enums;
using Common.ExceptionType.CustomException;
using Domain.Entities.ClassEntities;
using Domain.Entities.TeacherEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Concrete.TeacherService.TeacherClassService;

public class TeacherClassService : ServiceBase<TeacherClassService>, ITeacherClassService
{
    private readonly IRepository<Class> _classRepository;
    private readonly IRepository<Teacher> _teacherRepository;
    private readonly ICustomLoggerService<TeacherClassService> _logger;


    public TeacherClassService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor,
        ICustomLoggerService<TeacherClassService> logger) : base(contextAccessor)
    {
        _classRepository = unitOfWork.GetRepository<Class>();
        _teacherRepository = unitOfWork.GetRepository<Teacher>();
        _logger = logger;
    }

    public Task<List<SelectOptionViewModel>> GetAllClass()
    {
        var teacherClasses =
            _classRepository.DeferredWhere(x => x.Teacher != null && x.Teacher.UserId == CurrentUserId);

        return Task.FromResult(teacherClasses.Select(x => new SelectOptionViewModel
        {
            Id = x.Id,
            Title = x.Title
        }).ToList());
    }

    public Task<ResponseGetAllClassByFilterViewModel> GetAllClassByFilter(RequestGetAllClassByFilterViewModel model)
    {
        var teacherClasses = _classRepository.DeferredWhere(x => x.Teacher != null && x.Teacher.UserId == CurrentUserId)
            .Include(x => x.Students).AsQueryable();

        teacherClasses = FilterTeacherClasses(teacherClasses, model);

        var teacherClassesPaginated = teacherClasses.Paginate(model);

        return Task.FromResult(new ResponseGetAllClassByFilterViewModel
        {
            Count = teacherClassesPaginated.Count(),
            CurrentPage = model.Page,
            TotalCount = teacherClasses.Count(),
            Items = teacherClassesPaginated.Select(x => new ShowClassForTeacher
            {
                Id = x.Id,
                Description = x.Description,
                Semester = x.Semester,
                SemesterTitle = x.Semester.GetEnumDescription(),
                TotalAllowedStudent = x.TotalAllowedStudent,
                UniversityName = x.UniversityName,
                CreationDate = x.InsertTime.ConvertMiladiToJalali(),
                CurrentStudents = x.Students.Count
            }).ToList()
        });
    }

    public Task<int> SetClass(RequestSetClassViewModel model)
    {
        #region UPDATE CLASS

        if (model.ClassId > 0)
        {
            var resultUpdate = UpdateClass(model);
            return resultUpdate;
        }

        #endregion

        #region CREATE NEW CLASS

        var result = AddClass(model);
        return result;

        #endregion
    }

    public Task<bool> RemoveClass(int classId)
    {
        var classRoom =
            _classRepository.DeferredWhere(x => x.Id == classId && x.Teacher.UserId == CurrentUserId)
                .Include(x => x.Teacher)
                .FirstOrDefault() ?? throw new NotFoundException();

        if (classRoom.Teacher.UserId != CurrentUserId) throw new FormValidationException(MessageId.AccessToClassDenied);

        try
        {
            _classRepository.RemoveAsync(classRoom, true);
            _logger.LogAddSuccess("Class", classRoom.Id);
            return Task.FromResult(true);
        }
        catch (Exception e)
        {
            _logger.LogAddError(e, "Class");
            throw e ?? throw new ErrorException();
        }
    }


    private Task<int> AddClass(RequestSetClassViewModel model)
    {
        var newClass = new Class
        {
            Description = model.Description,
            Semester = model.Semester,
            TotalAllowedStudent = model.TotalAllowedStudent,
            UniversityName = model.UniversityName,
            Title = model.Title,
            TeacherId = _teacherRepository.DeferredWhere(x => x.UserId == CurrentUserId).Select(x => x.Id)
                .FirstOrDefault()
        };

        try
        {
            _classRepository.AddAsync(newClass);
            _logger.LogAddSuccess("Class", newClass.Id);
            return Task.FromResult(newClass.Id);
        }
        catch (Exception exception)
        {
            _logger.LogAddError(exception, "Class");
            throw exception ?? throw new ErrorException();
        }
    }

    private Task<int> UpdateClass(RequestSetClassViewModel model)
    {
        var classRoom = _classRepository.DeferredWhere(x => x.Id == model.ClassId).Include(x => x.Teacher)
                            .FirstOrDefault() ??
                        throw new NotFoundException();

        if (classRoom.Teacher.UserId != CurrentUserId) throw new FormValidationException(MessageId.AccessToClassDenied);

        classRoom.TotalAllowedStudent = model.TotalAllowedStudent;
        classRoom.UniversityName = model.UniversityName;
        classRoom.Semester = model.Semester;
        classRoom.Title = model.Title;
        classRoom.Description = model.Description;

        try
        {
            _classRepository.UpdateAsync(classRoom, true);
            _logger.LogUpdateSuccess("Class", classRoom.Id);
            return Task.FromResult(classRoom.Id);
        }
        catch (Exception exception)
        {
            _logger.LogUpdateError(exception, "Class", classRoom.Id);
            throw exception ?? throw new ErrorException();
        }

        //todo CHECK THAT STUDENTS IN CLASS ARE BELOW MODEL
        //if(model.TotalAllowedStudent > clas)
    }


    private static IQueryable<Class> FilterTeacherClasses(
        IQueryable<Class> teacherClasses,
        RequestGetAllClassByFilterViewModel model)
    {
        if (!string.IsNullOrEmpty(model.Title))
            teacherClasses = teacherClasses.Where(x => x.Title.Contains(model.Title));

        if (!string.IsNullOrEmpty(model.Description))
            teacherClasses = teacherClasses.Where(x => x.Description.Contains(model.Description));


        if (!string.IsNullOrEmpty(model.UniversityName))
            teacherClasses = teacherClasses.Where(x => x.UniversityName.Contains(model.UniversityName));


        if (model.Semester != null)
            teacherClasses = teacherClasses.Where(x => x.Semester == model.Semester);


        return teacherClasses;
    }
}