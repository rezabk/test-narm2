using Application.IRepositories;
using Application.Services.Base;
using Application.Services.Interface.Logger;
using Application.Services.Interface.TeacherService.TeacherPracticeStudentAnswerService;
using Application.ViewModels.Practice;
using Common.ExceptionType.CustomException;
using Domain.Entities.PracticeEntities;
using Domain.Entities.UserAgg;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Concrete.TeacherService.TeacherPracticeStudentAnswerService;

public class TeacherPracticeStudentAnswerService : ServiceBase<TeacherPracticeStudentAnswerService>,
    ITeacherPracticeStudentAnswerService
{
    private readonly IRepository<Practice> _practiceRepository;
    private readonly IRepository<PracticeQuestionAnswer> _practiceQuestionAnswerRepository;
    private readonly IRepository<PracticeQuestion> _practiceQuestionRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICustomLoggerService<TeacherPracticeStudentAnswerService> _logger;

    public TeacherPracticeStudentAnswerService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor,
        UserManager<ApplicationUser> userManager,
        ICustomLoggerService<TeacherPracticeStudentAnswerService> logger) : base(
        contextAccessor)
    {
        _practiceRepository = unitOfWork.GetRepository<Practice>();
        _practiceQuestionAnswerRepository = unitOfWork.GetRepository<PracticeQuestionAnswer>();
        _practiceQuestionRepository = unitOfWork.GetRepository<PracticeQuestion>();
        _userManager = userManager;
        _logger = logger;
    }


    public Task<ShowPracticeAnswer> GetAllPracticeAnswerByUserId(int practiceId, int userId)
    {
        var answers = _practiceQuestionAnswerRepository.DeferredWhere(x =>
                x.UserId == userId && x.PracticeQuestion.Practice.Id == practiceId &&
                x.PracticeQuestion.Practice.Class.Teacher.UserId == CurrentUserId).Include(x => x.PracticeQuestion)
            .ThenInclude(x => x.Practice)
            .Include(x => x.User);

        var practice =
            _practiceRepository.DeferredWhere(x => x.Class.Teacher.UserId == CurrentUserId && x.Id == practiceId)
                .FirstOrDefault() ??
            throw new NotFoundException();

        var user = _userManager.Users.FirstOrDefault(x => x.Id == userId) ?? throw new NotFoundException();

        return Task.FromResult(new ShowPracticeAnswer
        {
            UserId = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PracticeId = practice.Id,
            PracticeTitle = practice.Title,
            StudentNumber = user.StudentId,
            QuestionAnswerObjects = answers.Select(x => new PracticeQuestionAnswerObject
            {
                QuestionId = x.PracticeQuestionId,
                QuestionTitle = x.PracticeQuestion.Title,
                Answer = x.Description,
                Score = x.Score
            }).ToList()
        });
    }

    public Task<List<UserAnsweredList>> GetAllUserAnswered(int practiceId)
    {
        var practice =
            _practiceRepository.DeferredWhere(x => x.Class.Teacher.UserId == CurrentUserId && x.Id == practiceId)
                .Include(x => x.PracticeQuestions).ThenInclude(x => x.PracticeQuestionAnswers).ThenInclude(x => x.User)
                .FirstOrDefault() ??
            throw new NotFoundException();

        return Task.FromResult(practice.PracticeQuestions.SelectMany(x => x.PracticeQuestionAnswers).Select(x =>
            new UserAnsweredList
            {
                StudentId = x.User.StudentId,
                UserId = x.UserId,
                FullName = x.User.FirstName + " " + x.User.LastName
            }).ToList());
    }

    public Task<bool> ScorePracticeQuestionAnswer(int practiceQuestionAnswerId, double score)
    {
        var practiceQuestionAnswer =
            _practiceQuestionAnswerRepository
                .DeferredWhere(x =>
                    x.Id == practiceQuestionAnswerId &&
                    x.PracticeQuestion.Practice.Class.Teacher.UserId == CurrentUserId)
                .FirstOrDefault() ?? throw new NotFoundException();

        practiceQuestionAnswer.Score = score;

        try
        {
            _practiceQuestionAnswerRepository.UpdateAsync(practiceQuestionAnswer, true);
            _logger.LogUpdateSuccess("PracticeQuestionAnswer", practiceQuestionAnswer.Id);
            return Task.FromResult(true);
        }
        catch (Exception exception)
        {
            _logger.LogUpdateError(exception, "PracticeQuestionAnswer", practiceQuestionAnswer.Id);
            throw exception ?? throw new NotFoundException();
        }
    }
}