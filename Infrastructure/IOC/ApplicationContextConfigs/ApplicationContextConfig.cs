using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.IOC.ApplicationContextConfigs;

public static class ApplicationContextConfigs
{
    public static IServiceCollection AddApplicationContext<TService, TContext>
    (this IServiceCollection services,
        IConfiguration Configuration,
        string ConnectionConfigurationKey)
        where TService : class
        where TContext : DbContext, TService
    {
        services.AddDbContext<TContext>(option =>
        {
            option.UseNpgsql(
                ConnectionConfigurationKey,
                serverDbContextOptionsBuilder =>
                {
                    var minutes = (int)TimeSpan.FromMinutes(3).TotalSeconds;
                    serverDbContextOptionsBuilder.CommandTimeout(minutes);
                    serverDbContextOptionsBuilder.EnableRetryOnFailure();
                    //serverDbContextOptionsBuilder.UseNetTopologySuite();
                }
            );
        });

        services.AddScoped<TService, TContext>();

        return services;
    }
}