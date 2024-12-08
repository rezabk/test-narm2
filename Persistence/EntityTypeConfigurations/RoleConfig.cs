using System;
using System.Linq;
using Common.Enums;
using Common.Enums.RolesManagment;
using Domain.Entities.UserAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RoleConfig : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        var userRoles = Enum.GetValues(typeof(UserRolesEnum)).Cast<UserRolesEnum>();
        foreach (var userRole in userRoles)
            builder.HasData(new ApplicationRole
            {
                Id = userRole.GetId(),
                Name = Enum.GetName(userRole),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                NormalizedName = Enum.GetName(userRole)?.Normalize().ToUpper()
            });
    }
}