using System.ComponentModel.DataAnnotations.Schema;
using Common.Enums.TeacherEnum;
using Domain.Attributes;
using Domain.Entities.BaseAgg;
using Domain.Entities.UserAgg;

namespace Domain.Entities.TeacherEntities;

[EntityType]
[Auditable]
public class TeacherRequest : EntityBaseKeyInteger
{
    public int UserId { get; set; }
    
    public virtual ApplicationUser User { get; set; }
    
    public string? Description { get; set; }
    
    public string? AdminDescription { get; set; }

    public TeacherFieldEnum Field { get; set; }
    
    public TeacherRequestStatusEnum Status { get; set; }
    
    [Column("InsertTime")] public DateTime? InsertTime { get; set; }
}