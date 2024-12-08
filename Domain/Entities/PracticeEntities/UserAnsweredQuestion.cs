using Domain.Attributes;
using Domain.Entities.BaseAgg;
using Domain.Entities.UserAgg;

namespace Domain.Entities.PracticeEntities;

[EntityType]
[Auditable]
public class UserAnsweredQuestion : EntityBaseKeyInteger
{
    public int UserId { get; set; }

    public virtual ApplicationUser User { get; set; }

    public int PracticeQuestionId { get; set; }

    public virtual PracticeQuestion PracticeQuestion { get; set; }
}