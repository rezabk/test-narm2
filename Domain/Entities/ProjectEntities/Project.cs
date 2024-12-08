using Domain.Attributes;
using Domain.Entities.BaseAgg;
using Domain.Entities.ClassEntities;

namespace Domain.Entities.ProjectEntities;
[EntityType]
[Auditable]
public class Project  :EntityBaseKeyInteger
{
    public int ClassId { get; set; }

    public virtual Class Class { get; set; }

    public string Title { get; set; }

    public string? Description { get; set; }
    
    public string FileName { get; set; }
    
    public string FileExtension { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }
    
    public virtual ICollection<ProjectAnswer> ProjectAnswers { get; set; }
}