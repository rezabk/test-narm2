using System;

namespace Domain.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class EntityTypeAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Class)]
public class IdentityEntityTypeAttribute : Attribute
{
}