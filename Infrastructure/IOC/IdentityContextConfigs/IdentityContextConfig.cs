using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.IOC.IdentityContextConfigs;

public static class IdentityContextConfig
{
    public static IServiceCollection AddIdentityContext<TContext, TUser, TRole, TKey, TService /*, TDescriber*/>
    (this IServiceCollection services,
        IConfiguration Configuration,
        string ConnectionConfigurationKey,
        bool requireDigit = true,
        int requiredLength = 8,
        bool requireLowercase = true,
        bool requireNonAlphanumeric = true,
        bool requireUppercase = true,
        int requiredUniqueChars = 1,
        bool requireUniqueEmail = false,
        int maxFailedAccessAttempts = 3,
        int defaultLockoutTimeSpanFromMinutes = 30)
        where TContext : IdentityDbContext<TUser, TRole, TKey>, TService
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
        where TService : class
    //where TDescriber : IdentityErrorDescriber
    {
        services.AddDbContext<TContext>(options =>
        {
            options.UseNpgsql(
                ConnectionConfigurationKey,
                serverDbContextOptionsBuilder =>
                {
                    var minutes = (int)TimeSpan.FromMinutes(3).TotalSeconds;
                    serverDbContextOptionsBuilder.CommandTimeout(minutes);
                    serverDbContextOptionsBuilder.EnableRetryOnFailure();
                    //serverDbContextOptionsBuilder.UseNetTopologySuite();
                });
        });

        //AddIdentity => Microsoft.ASpNetCore.Identity
        services.AddIdentity<TUser, TRole>()
            .AddEntityFrameworkStores<TContext>()
            .AddDefaultTokenProviders()
            .AddRoles<TRole>()
            .AddErrorDescriber<CustomIdentityError>();
        //.AddErrorDescriber<TDescriber>();


        services.Configure<IdentityOptions>(options =>
        {
            //PassWord
            options.Password.RequireDigit = requireDigit;
            options.Password.RequiredLength = requiredLength;
            options.Password.RequireLowercase = requireLowercase;
            options.Password.RequireNonAlphanumeric = requireNonAlphanumeric;
            options.Password.RequireUppercase = requireUppercase;
            options.Password.RequiredUniqueChars = requiredUniqueChars;
            //User
            //options.User.AllowedUserNameCharacters = "0123456789";
            options.User.RequireUniqueEmail = requireUniqueEmail;
            //Lockout
            options.Lockout.MaxFailedAccessAttempts = maxFailedAccessAttempts;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(defaultLockoutTimeSpanFromMinutes);
        });

        services.AddScoped<TService, TContext>();

        return services;
    }
}