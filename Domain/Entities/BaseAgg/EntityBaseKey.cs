using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities.BaseAgg;

public class EntityBaseKey<TPrimaryKey>
{
    [Key] [Column] [NotNull] public virtual TPrimaryKey Id { get; set; }
}

public class EntityBaseKeyInteger : EntityBaseKey<int>
{
}
