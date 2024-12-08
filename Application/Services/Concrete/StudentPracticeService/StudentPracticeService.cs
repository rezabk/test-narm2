using Application.Cross.Interface;
using Application.IRepositories;
using Application.Services.Base;
using Application.Services.Interface.Logger;
using Application.Services.Interface.StudentPracticeService;
using Application.ViewModels.Practice;
using Application.ViewModels.PracticeQuestion;
using Application.ViewModels.Public;
using Common;
using Common.Enums;
using Common.ExceptionType.CustomException;
using Domain.Entities.ClassEntities;
using Domain.Entities.PracticeEntities;
using Domain.Entities.UserAgg;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Services.Concrete.StudentPracticeService;

public class StudentPracticeService : ServiceBase<StudentPracticeService>, IStudentPracticeService
{
    private readonly IRepository<Practice> _practiceRepository;
    private readonly IRepository<PracticeQuestion> _practiceQuestionRepository;
    private readonly IRepository<Class> _classRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly ILiaraUploader _uploader;
    private readonly ICustomLoggerService<StudentPracticeService> _logger;

    public StudentPracticeService(IUnitOfWork unitOfWork, IConfiguration configuration, ILiaraUploader uploader,
        IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager,
        IWebHostEnvironment hostingEnvironment,
        ICustomLoggerService<StudentPracticeService> logger) : base(httpContextAccessor)
    {
        _practiceRepository = unitOfWork.GetRepository<Practice>();
        _practiceQuestionRepository = unitOfWork.GetRepository<PracticeQuestion>();
        _classRepository = unitOfWork.GetRepository<Class>();
        _configuration = configuration;
        _hostingEnvironment = hostingEnvironment;
        _uploader = uploader;
        _logger = logger;
        _userManager = userManager;
    }

    public Task<List<ShowPracticeByClassId>> GetAllPracticeByClassId(int classId)
    {
        var practices = _practiceRepository.DeferredWhere(x =>
                x.Class != null && x.ClassId == classId && x.Class.Students.Select(y => y.Id).Contains(CurrentUserId))
            .Include(x => x.PracticeQuestions);


        return Task.FromResult(practices.Select(x => new ShowPracticeByClassId
        {
            Id = x.Id,
            Description = x.Description,
            ClassId = x.ClassId,
            ClassTitle = x.Class.Title,
            EndDate = x.EndDate.ConvertMiladiToJalali(),
            StartDate = x.StartDate.ConvertMiladiToJalali(),
            PracticeQuestionsCount = x.PracticeQuestions.Count
        }).ToList());
    }

    public Task<List<ShowPracticeQuestionViewModel>> GetAllPracticeQuestionByPracticeId(int practiceId)
    {
        var practiceQuestions =
            _practiceQuestionRepository.DeferredWhere(x => x.Practice != null && x.PracticeId == practiceId &&
                                                           x.Practice.Class.Students.Select(x => x.Id)
                                                               .Contains(CurrentUserId));


        return Task.FromResult(practiceQuestions.Select(x => new ShowPracticeQuestionViewModel
        {
            Id = x.Id,
            Description = x.Description,
            FileName = x.FileName,
            Title = x.Title
        }).ToList());
    }

    public Task<bool> AnswerPracticeQuestion(RequestAnswerPracticeQuestionViewModel model)
    {
        var practiceQuestion =
            _practiceQuestionRepository.DeferredWhere(x => x.Practice != null && x.Id == model.PracticeQuestionId &&
                                                           x.Practice.Class.Students.Select(x => x.Id)
                                                               .Contains(CurrentUserId))
                .Include(x => x.UserAnsweredQuestions).Include(x => x.PracticeQuestionAnswers).Include(x => x.Practice)
                .FirstOrDefault() ?? throw new NotFoundException();

        if (practiceQuestion.UserAnsweredQuestions.Select(x => x.UserId).Contains(CurrentUserId))
            throw new FormValidationException(MessageId.AlreadyAnsweredQuestion);

        if (DateTime.UtcNow > practiceQuestion.Practice.EndDate)
            throw new FormValidationException(MessageId.DeadlineReached);

        var newPracticeQuestionAnswer = new PracticeQuestionAnswer
            { PracticeQuestionId = model.PracticeQuestionId, Description = model.Description, UserId = CurrentUserId };

        try
        {
            practiceQuestion.PracticeQuestionAnswers.Add(newPracticeQuestionAnswer);
            practiceQuestion.UserAnsweredQuestions.Add(new UserAnsweredQuestion
                { PracticeQuestionId = practiceQuestion.Id, UserId = CurrentUserId });

            _practiceQuestionRepository.UpdateAsync(practiceQuestion, true);
            _logger.LogUpdateSuccess("PracticeQuestion", practiceQuestion.Id);
            return Task.FromResult(true);
        }
        catch (Exception exception)
        {
            _logger.LogUpdateError(exception, "PracticeQuestion", practiceQuestion.Id);
            throw exception ?? throw new ErrorException();
        }
    }

    public Task<ResponseGetFileViewModel> GetQuestionImage(string fileName)
    {
        var practice =
            _practiceQuestionRepository.DeferredWhere(x => x.Practice != null && x.FileName == fileName &&
                                                           x.Practice.Class.Students.Select(x => x.Id)
                                                               .Contains(CurrentUserId))
                .FirstOrDefault() ?? throw new NotFoundException();

        return Task.FromResult(new ResponseGetFileViewModel
        {
            FileName = practice.FileName,
            MemoryStream = _uploader.Get(_configuration.GetSection("File:FilePracticeQuestions").Value,
                practice.FileName, null).Result
        });
    }
}