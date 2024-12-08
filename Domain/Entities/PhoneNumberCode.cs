using System;
using Domain.Attributes;
using Domain.Entities.BaseAgg;

namespace Domain.Entities;

[EntityType]
[Auditable]
public class PhoneNumberCode : EntityBaseKey<int>
{
    public string PhoneNumber { get; set; }

    public int GeneratedCode { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime ExpireDate { get; set; }

    public bool IsUsed { get; set; } = false;
}