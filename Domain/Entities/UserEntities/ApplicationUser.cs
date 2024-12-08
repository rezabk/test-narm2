using Domain.Entities.ClassEntities;
using Domain.Entities.PracticeEntities;
using Domain.Entities.TeacherEntities;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.UserAgg;

public class ApplicationUser : IdentityUser<int>
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime? Birthday { get; set; }

    public string? StudentId { get; set; }

    public string? ProfileImageFileName { get; set; }

    public string? ProfileImageFileExtension { get; set; }

    public virtual ICollection<Teacher> Teachers { get; set; }

    public virtual ICollection<Class> Classes { get; set; }

    public virtual ICollection<PracticeQuestionAnswer> PracticeQuestionAnswers { get; set; }
    
    public virtual ICollection<UserAnsweredQuestion> UserAnsweredQuestions { get; set; }
    
    public virtual ICollection<TeacherRequest> TeacherRequests { get; set; }
}