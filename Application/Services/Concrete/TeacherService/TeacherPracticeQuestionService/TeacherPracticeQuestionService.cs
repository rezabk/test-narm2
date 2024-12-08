using Application.Cross.Interface;
using Application.IRepositories;
using Application.Services.Base;
using Application.Services.Interface.Logger;
using Application.Services.Interface.TeacherService.TeacherPracticeQuestionService;
using Application.ViewModels.PracticeQuestion;
using Application.ViewModels.Public;
using Common.ExceptionType.CustomException;
using Domain.Entities.PracticeEntities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Application.Services.Concrete.TeacherService.TeacherPracticeQuestionService;

public class TeacherPracticeQuestionService : ServiceBase<TeacherPracticeQuestionService>,
    ITeacherPracticeQuestionService
{
    private readonly IRepository<PracticeQuestion> _practiceQuestionRepository;
    private readonly ICustomLoggerService<TeacherPracticeQuestionService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly ILiaraUploader _uploader;

    public TeacherPracticeQuestionService(IUnitOfWork unitOfWork,
        ICustomLoggerService<TeacherPracticeQuestionService> logger,
        IConfiguration configuration, IWebHostEnvironment hostingEnvironment, ILiaraUploader uploader,
        IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _practiceQuestionRepository = unitOfWork.GetRepository<PracticeQuestion>();
        _logger = logger;
        _uploader = uploader;
        _configuration = configuration;
        _hostingEnvironment = hostingEnvironment;
    }

    public Task<List<ShowPracticeQuestionViewModel>> GetAllPracticeQuestionByPracticeId(int practiceId)
    {
        var practiceQuestions =
            _practiceQuestionRepository.DeferredWhere(x =>
                x.Practice != null && x.PracticeId == practiceId && x.Practice.Class.Teacher.UserId == CurrentUserId);

        return Task.FromResult(practiceQuestions.Select(x => new ShowPracticeQuestionViewModel
        {
            Id = x.Id,
            Description = x.Description,
            FileName = x.FileName,
            Title = x.Title
        }).ToList());
    }

    public Task<int> SetQuestion(RequestSetQuestionViewModel model)
    {
        #region UPDATE QUESTION

        if (model.Id > 0)
        {
            var practiceQuestion = _practiceQuestionRepository
                                       .DeferredWhere(x =>
                                           x.Practice != null &&
                                           x.Practice.Class.Teacher.UserId == CurrentUserId &&
                                           x.Id == model.Id && x.PracticeId == model.PracticeId)
                                       .FirstOrDefault() ??
                                   throw new NotFoundException();


            practiceQuestion.Description = model.Description;
            practiceQuestion.Title = model.Title;

            if (!string.IsNullOrWhiteSpace(practiceQuestion.FileName)) _ = DeleteFile(practiceQuestion.FileName);
            if (model.File != null)
            {
                var filePath = UploadFile(model.File).Result;
                practiceQuestion.FileName = string.IsNullOrEmpty(filePath) ? null : Path.GetFileName(filePath);
                practiceQuestion.FileExtension = string.IsNullOrEmpty(filePath) ? null : Path.GetExtension(filePath);
            }

            try
            {
                _practiceQuestionRepository.UpdateAsync(practiceQuestion, true);
                _logger.LogUpdateSuccess("PracticeQuestion", practiceQuestion.Id);
                return Task.FromResult(practiceQuestion.Id);
            }
            catch (Exception exception)
            {
                _logger.LogUpdateError(exception, "PracticeQuestion", practiceQuestion.Id);
                throw exception ?? throw new ErrorException();
            }
        }

        #endregion


        #region ADD NEW QUESTION

        var newQuestion = new PracticeQuestion
        {
            Title = model.Title,
            Description = model.Description,
            PracticeId = model.PracticeId,
        };

        if (model.File != null)
        {
            var filePath = UploadFile(model.File).Result;
            newQuestion.FileName = string.IsNullOrEmpty(filePath) ? null : Path.GetFileName(filePath);
            newQuestion.FileExtension = string.IsNullOrEmpty(filePath) ? null : Path.GetExtension(filePath);
        }

        try
        {
            _practiceQuestionRepository.AddAsync(newQuestion);
            _logger.LogAddSuccess("PracticeQuestion", newQuestion.Id);
            return Task.FromResult(newQuestion.Id);
        }
        catch (Exception exception)
        {
            _logger.LogAddError(exception, "PracticeQuestion");
            throw exception ?? throw new ErrorException();
        }

        #endregion
    }

    public Task<bool> RemoveQuestion(int practiceQuestionId)
    {
        var question = _practiceQuestionRepository.DeferredWhere(x =>
                x.Practice != null && x.Id == practiceQuestionId && x.Practice.Class.Teacher.UserId == CurrentUserId)
            .FirstOrDefault() ?? throw new NotFoundException();

        try
        {
            _practiceQuestionRepository.RemoveAsync(question, true);
            _logger.LogRemoveSuccess("PracticeQuestion", question.Id);
            return Task.FromResult(true);
        }
        catch (Exception exception)
        {
            _logger.LogRemoveError(exception, "PracticeQuestion", question.Id);
            throw exception ?? throw new ErrorException();
        }
    }

    public Task<ResponseGetFileViewModel> GetQuestionImage(string fileName)
    {
        var practice =
            _practiceQuestionRepository.DeferredWhere(x => x.Practice != null && x.FileName == fileName)
                .FirstOrDefault() ?? throw new NotFoundException();

        return Task.FromResult(new ResponseGetFileViewModel
        {
            FileName = practice.FileName,
            MemoryStream = _uploader.Get(_configuration.GetSection("File:FilePracticeQuestions").Value,
                practice.FileName, null).Result
        });
    }

    private async Task<string> UploadFile(IFormFile file)
    {
        var fileName = await _uploader.Upload(_configuration.GetSection("File:FilePracticeQuestions").Value, file,
            null);
        return fileName;
    }

    private async Task<bool> DeleteFile(string fileName)
    {
        await _uploader.Delete(_configuration.GetSection("File:FilePracticeQuestions").Value, fileName, null);
        return true;
    }
}