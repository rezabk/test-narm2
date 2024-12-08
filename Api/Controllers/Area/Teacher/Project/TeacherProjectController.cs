

using Application.Services.Interface.TeacherService.TeacherProjectService;
using Application.ViewModels.Project;
using Common.Enums.RolesManagment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Area.Teacher.Project;

[Area("Teacher")]
[Authorize(Roles = nameof(UserRolesEnum.Teacher))]
[Route("/api/teacher/project")]
public class TeacherProjectController  :BaseController
{
    private readonly ITeacherProjectService _teacherProjectService;

    
    
    public TeacherProjectController(ITeacherProjectService teacherProjectService)
    {
        _teacherProjectService = teacherProjectService;
    }
    
    [HttpGet("[action]")]
    public async Task<List<ShowProjectViewModel>> GetAllProjectByClassId(int classId)
    {
        return await _teacherProjectService.GetAllProjectByClassId(classId);
    }
        
    [HttpPost("[action]")]
    public async Task<int> SetProject([FromForm] RequestSetProjectViewModel model)
    {
        return await _teacherProjectService.SetProject(model);
    }
    
    [HttpDelete("[action]")]
    public async Task<bool> RemoveProject(int projectId)
    {
        return await _teacherProjectService.RemoveProject(projectId);
    }
    
    [HttpGet("[action]")]
    public Task<IResult> ServeQuestionImage(string fileName)
    {
        var response = _teacherProjectService.GetProjectFile(fileName).Result;
        return Task.FromResult(Results.File(response.MemoryStream.ToArray(), "application/octet-stream",
            Path.GetFileName(response.FileName)));
    }
}