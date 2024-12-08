using Microsoft.AspNetCore.Http;

namespace Application.ViewModels.PracticeQuestion;

public class RequestAnswerPracticeQuestionViewModel
{
    public int PracticeQuestionId { get; set; }
    
    public string Description { get; set; }
    
}