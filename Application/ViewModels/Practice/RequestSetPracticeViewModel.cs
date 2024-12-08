namespace Application.ViewModels.Practice;

public class RequestSetPracticeViewModel
{
    public int PracticeId { get; set; }
    
    public int ClassId { get; set; }
    
    public string Title { get; set; }
    
    public string? Description { get; set; }
    
    public string StartDate { get; set; }

    public string? EndDate { get; set; }
}