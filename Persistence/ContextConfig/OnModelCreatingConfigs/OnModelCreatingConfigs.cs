using Microsoft.EntityFrameworkCore;

namespace Persistence.ContextConfig.OnModelCreatingConfigs;

public class OnModelCreatingConfigs
{
    public static void AddConfigurations(ModelBuilder builder)
    {
        // builder.ApplyConfiguration(new UserMachineConfig());
    }
}