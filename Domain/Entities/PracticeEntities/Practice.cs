using Domain.Attributes;
using Domain.Entities.BaseAgg;
using Domain.Entities.ClassEntities;

namespace Domain.Entities.PracticeEntities;

[EntityType]
[Auditable]
public class Practice : EntityBaseKeyInteger
{
    public int ClassId { get; set; }

    public virtual Class Class { get; set; }

    public string Title { get; set; }

    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }
    
    public virtual ICollection<PracticeQuestion> PracticeQuestions { get; set; }
}