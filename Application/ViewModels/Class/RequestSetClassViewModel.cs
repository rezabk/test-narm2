using Common.Enums.ClassEnum;

namespace Application.ViewModels.Class;

public class RequestSetClassViewModel
{
    public int ClassId { get; set; }
    
    public string Title { get; set; }
    
    public string? Description { get; set; }

    public string? UniversityName { get; set; }

    public int TotalAllowedStudent { get; set; }

    public ClassSemesterEnum Semester { get; set; }
}