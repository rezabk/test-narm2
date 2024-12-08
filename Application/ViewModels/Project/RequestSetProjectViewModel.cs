using Microsoft.AspNetCore.Http;

namespace Application.ViewModels.Project;

public class RequestSetProjectViewModel
{
    public int ProjectId { get; set; }
    public int ClassId { get; set; }
    
    public string Title { get; set; }
    
    public string? Description { get; set; }
    
    public string StartDate { get; set; }

    public string? EndDate { get; set; }
    
    public IFormFile File { get; set; }
}