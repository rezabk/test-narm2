using Application.IRepositories;
using Application.Services.Base;
using Application.Services.Interface.Logger;
using Application.Services.Interface.TeacherService.TeacherPracticeService;
using Application.ViewModels.Practice;
using Common;
using Common.Enums;
using Common.ExceptionType.CustomException;
using Domain.Entities.PracticeEntities;
using Domain.Entities.TeacherEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Concrete.TeacherService.TeacherPracticeService;

public class TeacherPracticeService : ServiceBase<TeacherPracticeService>, ITeacherPracticeService
{
    private readonly IRepository<Practice> _practiceRepository;
    private readonly IRepository<Teacher> _teacherRepository;
    private readonly ICustomLoggerService<TeacherPracticeService> _logger;


    public TeacherPracticeService(IUnitOfWork unitOfWork, ICustomLoggerService<TeacherPracticeService> logger,
        IHttpContextAccessor contextAccessor) : base(contextAccessor)
    {
        _practiceRepository = unitOfWork.GetRepository<Practice>();
        _teacherRepository = unitOfWork.GetRepository<Teacher>();
        _logger = logger;
    }

    public Task<List<ShowPracticeByClassId>> GetAllPracticeByClassId(int classId)
    {
        var practices = _practiceRepository.DeferredWhere(x =>
                x.Class != null && x.ClassId == classId && x.Class.Teacher.UserId == CurrentUserId)
            .Include(x => x.Class)
            .Include(x => x.PracticeQuestions);

        return Task.FromResult(practices.Select(x => new ShowPracticeByClassId
        {
            Id = x.Id,
            Description = x.Description,
            StartDate = x.StartDate.ConvertMiladiToJalali(),
            EndDate = x.EndDate.ConvertMiladiToJalali(),
            ClassId = x.ClassId,
            ClassTitle = x.Class.Title,
            PracticeQuestionsCount = x.PracticeQuestions.Count
        }).ToList());
    }


    public Task<int> SetPractice(RequestSetPracticeViewModel model)
    {
        #region UPDATE PRACTICE

        if (model.PracticeId > 0)
        {
            var practice = _practiceRepository.DeferredWhere(x => x.Id == model.PracticeId).FirstOrDefault() ??
                           throw new NotFoundException();
            practice.Description = model.Description;
            practice.Title = model.Title;
            practice.StartDate = model.StartDate.ConvertJalaliToMiladi();
            practice.EndDate = model.EndDate?.ConvertJalaliToMiladi();

            try
            {
                _practiceRepository.UpdateAsync(practice, true);
                _logger.LogUpdateSuccess("Practice", practice.Id);
                return Task.FromResult(practice.Id);
            }
            catch (Exception exception)
            {
                _logger.LogUpdateError(exception, "Practice", practice.Id);
                throw exception ?? throw new ErrorException();
            }
        }

        #endregion

        #region ADD PRACTICE

        var newPractice = new Practice
        {
            ClassId = model.ClassId,
            Description = model.Description,
            Title = model.Title,
            StartDate = model.StartDate.ConvertJalaliToMiladi(),
            EndDate = model.EndDate?.ConvertJalaliToMiladi(),
        };

        try
        {
            _practiceRepository.AddAsync(newPractice, true);
            _logger.LogAddSuccess("Practice", newPractice.Id);
            return Task.FromResult(newPractice.Id);
        }
        catch (Exception exception)
        {
            _logger.LogAddError(exception, "Practice");
            throw exception ?? throw new ErrorException();
        }

        #endregion
    }

    public Task<bool> RemovePractice(int practiceId)
    {
        var practice = _practiceRepository.DeferredWhere(x => x.Class != null && x.Id == practiceId)
                           .Include(x => x.Class)
                           .ThenInclude(x => x.Teacher)
                           .FirstOrDefault() ??
                       throw new NotFoundException();

        if (practice.Class.Teacher.UserId != CurrentUserId)
            throw new FormValidationException(MessageId.AccessToClassDenied);

        try
        {
            _practiceRepository.RemoveAsync(practice, true);
            _logger.LogRemoveSuccess("Practice", practice.Id);
            return Task.FromResult(true);
        }
        catch (Exception exception)
        {
            _logger.LogRemoveError(exception, "Practice", practice.Id);
            throw exception ?? throw new ErrorException();
        }
    }
}