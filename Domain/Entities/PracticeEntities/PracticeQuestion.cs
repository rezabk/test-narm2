using Domain.Attributes;
using Domain.Entities.BaseAgg;

namespace Domain.Entities.PracticeEntities;

[EntityType]
[Auditable]
public class PracticeQuestion : EntityBaseKeyInteger
{
    public int PracticeId { get; set; }

    public virtual Practice Practice { get; set; }

    public string Title { get; set; }

    public string? Description { get; set; }

    public string? FileName { get; set; }

    public string? FileExtension { get; set; }

    public virtual ICollection<PracticeQuestionAnswer> PracticeQuestionAnswers { get; set; }
    public virtual ICollection<UserAnsweredQuestion> UserAnsweredQuestions { get; set; }
}