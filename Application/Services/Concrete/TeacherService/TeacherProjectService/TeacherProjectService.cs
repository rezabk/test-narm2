using Application.Cross.Interface;
using Application.IRepositories;
using Application.Services.Base;
using Application.Services.Interface.Logger;
using Application.Services.Interface.TeacherService.TeacherProjectService;
using Application.ViewModels.Project;
using Application.ViewModels.Public;
using Common;
using Common.ExceptionType.CustomException;
using Domain.Entities.ProjectEntities;
using Domain.Entities.TeacherEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Services.Concrete.TeacherService.TeacherProjectService;

public class TeacherProjectService : ServiceBase<TeacherProjectService>, ITeacherProjectService
{
    private readonly IRepository<Project> _projectRepository;
    private readonly IRepository<Teacher> _teacherRepository;
    private readonly ICustomLoggerService<TeacherProjectService> _logger;
    private readonly ILiaraUploader _uploader;
    private readonly IConfiguration _configuration;

    public TeacherProjectService(IUnitOfWork unitOfWork, ICustomLoggerService<TeacherProjectService> logger,
        ILiaraUploader uploader,
        IHttpContextAccessor contextAccessor, IConfiguration configuration) : base(contextAccessor)
    {
        _projectRepository = unitOfWork.GetRepository<Project>();
        _teacherRepository = unitOfWork.GetRepository<Teacher>();
        _logger = logger;
        _uploader = uploader;
        _configuration = configuration;
    }

    public Task<List<ShowProjectViewModel>> GetAllProjectByClassId(int classId)
    {
        var projects =
            _projectRepository.DeferredWhere(x => x.ClassId == classId && x.Class.Teacher.UserId == CurrentUserId)
                .Include(x => x.Class);

        return Task.FromResult(projects.Select(x => new ShowProjectViewModel
        {
            Id = x.Id,
            Title = x.Title,
            StartDate = x.StartDate.ConvertMiladiToJalali(),
            EndDate = x.EndDate.ConvertMiladiToJalali(),
            Description = x.Description,
            FileName = x.FileName,
            ClassId = x.ClassId,
            ClassTitle = x.Class.Title
        }).ToList());
    }

    public Task<int> SetProject(RequestSetProjectViewModel model)
    {
        #region UPDATE PROJECT

        if (model.ProjectId > 0)
        {
            var project = _projectRepository.DeferredWhere(x => x.Id == model.ProjectId).FirstOrDefault() ??
                          throw new NotFoundException();

            project.Title = model.Title;
            project.Description = model.Description;
            project.StartDate = model.StartDate.ConvertJalaliToMiladi();
            project.EndDate = model.EndDate.ConvertJalaliToMiladi();

            _ = DeleteFile(project.FileName);
            var filePathUpdate = UploadFile(model.File).Result;
            project.FileName = string.IsNullOrEmpty(filePathUpdate) ? null : Path.GetFileName(filePathUpdate);
            project.FileExtension = string.IsNullOrEmpty(filePathUpdate) ? null : Path.GetExtension(filePathUpdate);

            try
            {
                _projectRepository.UpdateAsync(project, true);
                _logger.LogUpdateSuccess("Project", project.Id);
                return Task.FromResult(project.Id);
            }
            catch (Exception exception)
            {
                _logger.LogUpdateError(exception, "Project", project.Id);
                throw exception ?? throw new ErrorException();
            }
        }

        #endregion

        #region ADD PROJECT

        var newProject = new Project
        {
            ClassId = model.ClassId,
            Description = model.Description,
            Title = model.Title,
            StartDate = model.StartDate.ConvertJalaliToMiladi(),
            EndDate = model.EndDate.ConvertJalaliToMiladi(),
        };

        var filePath = UploadFile(model.File).Result;
        newProject.FileName = string.IsNullOrEmpty(filePath) ? null : Path.GetFileName(filePath);
        newProject.FileExtension = string.IsNullOrEmpty(filePath) ? null : Path.GetExtension(filePath);

        try
        {
            _projectRepository.AddAsync(newProject);
            _logger.LogAddSuccess("Project", newProject.Id);
            return Task.FromResult(newProject.Id);
        }
        catch (Exception exception)
        {
            _logger.LogAddError(exception, "Project");
            throw exception ?? throw new ErrorException();
        }

        #endregion
    }

    public Task<bool> RemoveProject(int projectId)
    {
        var project =
            _projectRepository.DeferredWhere(x => x.Id == projectId && x.Class.Teacher.UserId == CurrentUserId)
                .FirstOrDefault() ?? throw new NotFoundException();
        try
        {
            _projectRepository.RemoveAsync(project, true);
            _logger.LogRemoveSuccess("Project", project.Id);
            return Task.FromResult(true);
        }
        catch (Exception exception)
        {
            _logger.LogRemoveError(exception, "Project", project.Id);
            throw exception ?? throw new ErrorException();
        }
    }

    public Task<ResponseGetFileViewModel> GetProjectFile(string fileName)
    {
        var project =
            _projectRepository.DeferredWhere(x => x.FileName == fileName && x.Class.Teacher.UserId == CurrentUserId)
                .FirstOrDefault() ?? throw new NotFoundException();

        return Task.FromResult(new ResponseGetFileViewModel
        {
            FileName = project.FileName,
            MemoryStream = _uploader.Get(_configuration.GetSection("File:FileProject").Value,
                project.FileName, null).Result
        });
    }


    private async Task<string> UploadFile(IFormFile file)
    {
        var fileName = await _uploader.Upload(_configuration.GetSection("File:FileProject").Value, file, null);
        return fileName;
    }

    private async Task<bool> DeleteFile(string fileName)
    {
        await _uploader.Delete(_configuration.GetSection("File:FileProject").Value, fileName, null);
        return true;
    }
}