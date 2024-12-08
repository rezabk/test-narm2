using System;
using Domain.Attributes;
using Microsoft.EntityFrameworkCore;

namespace Persistence.ContextConfig.OnModelCreatingConfigs;

public class ReflectionEntitis
{
    public static void Execute(ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
            if (entityType.ClrType.GetCustomAttributes(typeof(AuditableAttribute), true).Length > 0)
            {
                builder.Entity(entityType.Name).Property<DateTime?>("InsertTime");
                builder.Entity(entityType.Name).Property<string>("InsertByUserId");
                builder.Entity(entityType.Name).Property<DateTime?>("UpdateTime");
                builder.Entity(entityType.Name).Property<string>("UpdateByUserId");
                builder.Entity(entityType.Name).Property<DateTime?>("RemoveTime");
                builder.Entity(entityType.Name).Property<bool?>("IsRemoved");
                builder.Entity(entityType.Name).Property<string>("RemoveByUserId");
            }
    }
}