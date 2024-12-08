using Domain.Entities.PracticeEntities;

namespace Application.ViewModels.Practice;

public class ShowPracticeByClassId
{
    public int Id { get; set; }

    public int ClassId { get; set; }

    public string ClassTitle { get; set; }

    public string? Description { get; set; }

    public string StartDate { get; set; }

    public string? EndDate { get; set; }

    public int PracticeQuestionsCount { get; set; }
}

public class ShowPracticeAnswer
{
    public int PracticeId { get; set; }

    public string PracticeTitle { get; set; }
    
    public int UserId { get; set; }
    
   public string StudentNumber { get; set; }
   
   public string FirstName { get; set; }
   
   public string LastName { get; set; }
   
   public List<PracticeQuestionAnswerObject> QuestionAnswerObjects { get; set; }
}

public class PracticeQuestionAnswerObject
{
    public int QuestionId { get; set; }
    
    public string QuestionTitle { get; set; }
    
    public string Answer { get; set; }
    
    public double Score { get; set; }
}

public class UserAnsweredList
{
    public int UserId { get; set; }
    
    public string StudentId { get; set; }
    
    public string FullName { get; set; }
}