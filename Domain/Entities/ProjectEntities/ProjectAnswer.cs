using Domain.Attributes;
using Domain.Entities.BaseAgg;
using Domain.Entities.UserAgg;

namespace Domain.Entities.ProjectEntities;
[EntityType]
[Auditable]
public class ProjectAnswer  :EntityBaseKeyInteger
{
    public int UserId { get; set; }
    
    public virtual ApplicationUser User { get; set; }
    
    public int ProjectId { get; set; }
    
    public virtual Project Project { get; set; }
   
    public string FileName { get; set; }
    
    public string FileExtension { get; set; }
}