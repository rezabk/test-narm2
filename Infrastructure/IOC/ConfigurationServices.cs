using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ConfigurationServices
{
    public static IServiceCollection AddAppConfigures(this IServiceCollection services,
        IConfiguration configuration)
    {
        return services;
    }

    public static IServiceCollection AddProductService(this IServiceCollection services)
    {
        ////Services
        // services.AddScoped<IProductCategoryService, ProductCategoryService>();
        // services.AddScoped<IProductService, ProductService>();

        //FluentValidation
        // services
        //     .AddTransient<IValidator<SetLegalPrimaryInformationViewModel>,
        //         SelLegalUserPrimaryInformationValidator>();
        // services
        //     .AddTransient<IValidator<SetFactoryPrimaryInformationViewModel>,
        //         SelLegalUserFactoryPrimaryInformationValidator>();
        // services
        //     .AddTransient<IValidator<SetRealUserPrimaryInformationViewModel>,
        //         SetRealUserPrimaryInformationValidator>();

        return services;
    }
}