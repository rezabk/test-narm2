using Microsoft.AspNetCore.Http;

namespace Application.ViewModels.PracticeQuestion;

public class RequestSetQuestionViewModel

{
    public int Id { get; set; }
    
    public int PracticeId { get; set; }

    public string Title { get; set; }

    public string? Description { get; set; }

    public IFormFile? File { get; set; }
}