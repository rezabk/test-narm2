using Application.Cross.Concrete;
using Application.Cross.Interface;
using Application.Services.Concrete.AccountService;
using Application.Services.Concrete.AccountService.ValidateCodeService;
using Application.Services.Concrete.Admin.AdminRoleService;
using Application.Services.Concrete.Admin.AdminTeacherRequestService;
using Application.Services.Concrete.Logger;
using Application.Services.Concrete.ProfileService;
using Application.Services.Concrete.ProfileService.ProfileValidator;
using Application.Services.Concrete.Sms;
using Application.Services.Concrete.StudentPracticeService;
using Application.Services.Concrete.StudentService;
using Application.Services.Concrete.TeacherRequestService;
using Application.Services.Concrete.TeacherService.TeacherClassService;
using Application.Services.Concrete.TeacherService.TeacherPracticeQuestionService;
using Application.Services.Concrete.TeacherService.TeacherPracticeService;
using Application.Services.Concrete.TeacherService.TeacherPracticeStudentAnswerService;
using Application.Services.Concrete.TeacherService.TeacherProjectService;
using Application.Services.Interface.AccountService;
using Application.Services.Interface.AccountService.AccountValidatorService;
using Application.Services.Interface.Admin.AdminRoleService;
using Application.Services.Interface.Admin.AdminTeacherRequestService;
using Application.Services.Interface.Logger;
using Application.Services.Interface.ProfileService;
using Application.Services.Interface.ProfileService.ProfileValidator;
using Application.Services.Interface.Sms;
using Application.Services.Interface.StudentPracticeService;
using Application.Services.Interface.StudentService;
using Application.Services.Interface.TeacherRequestService;
using Application.Services.Interface.TeacherService.TeacherClassService;
using Application.Services.Interface.TeacherService.TeacherPracticeQuestionService;
using Application.Services.Interface.TeacherService.TeacherPracticeService;
using Application.Services.Interface.TeacherService.TeacherPracticeStudentAnswerService;
using Application.Services.Interface.TeacherService.TeacherProjectService;
using Application.Services.Interface.Utils;
using Application.Utils;
using Application.ViewModels.Account;
using Application.ViewModels.Account.FluentValidation;
using Application.ViewModels.Account.Login;
using Application.ViewModels.Account.Login.FluentValidation;
using Application.ViewModels.Account.RegisterWithStudentId;
using Application.ViewModels.Account.RegisterWithStudentId.FluentValidation;
using Application.ViewModels.Admin.AdminRoleService;
using Application.ViewModels.Admin.AdminRoleService.FluentValidation;
using Application.ViewModels.Class;
using Application.ViewModels.Class.FluentValidation;
using Application.ViewModels.Practice;
using Application.ViewModels.Practice.FluentValidation;
using Application.ViewModels.PracticeQuestion;
using Application.ViewModels.PracticeQuestion.FluentValidation;
using Application.ViewModels.Profile.ChangePassword;
using Application.ViewModels.Profile.ChangePassword.FluentValidation;
using Application.ViewModels.Project;
using Application.ViewModels.Project.FluentValidation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ServiceRegistration
{
    public static IServiceCollection RegisterAllApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAdminRoleService, AdminRoleService>();
        
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IAccountValidatorService, AccountValidatorService>();
        services.AddScoped<IProfileValidator, ProfileValidator>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<ITeacherClassService, TeacherClassService>();
        services.AddScoped<ITeacherPracticeService, TeacherPracticeService>();
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IStudentPracticeService, StudentPracticeService>();
        services.AddScoped<ITeacherPracticeQuestionService, TeacherPracticeQuestionService>();
        services.AddScoped<ITeacherProjectService, TeacherProjectService>();
        services.AddScoped<ITeacherPracticeStudentAnswerService, TeacherPracticeStudentAnswerService>();
        services.AddScoped<ITeacherRequestService, TeacherRequestService>();
        services.AddScoped<ITeacherRequestAdminService, TeacherRequestAdminService>();
        

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUploader, Uploader>();
        services.AddScoped<ILiaraUploader, LiaraUploader>();
        services.AddScoped<ISmsService, SmsService>();

        services.AddSingleton(typeof(ICustomLoggerService<>), typeof(CustomLoggerService<>));
      


        services.AddLogging();

        return services;
    }

    public static IServiceCollection RegisterFluentValidationServices(this IServiceCollection services)
    {
        services.AddTransient<IValidator<RegisterOrLoginViewModel>, RegisterOrLoginValidator>();
    services.AddTransient<IValidator<ChangePasswordViewModel>, ChangePasswordValidator>();
        services.AddTransient<IValidator<RequestSetClassViewModel>, SetClassViewModelValidator>();
        services.AddTransient<IValidator<RequestAssignTeacherByAdminViewModel>, AssignTeacherByAdminValidator>();
        services.AddTransient<IValidator<RegisterViewModel>, RegisterValidator>();
        services.AddTransient<IValidator<LoginViewModel>, LoginValidator>();
        services.AddTransient<IValidator<RequestSetPracticeViewModel>, RequestSetPracticeValidator>();
        services.AddTransient<IValidator<RequestSetQuestionViewModel>, SetQuestionValidator>();
        services.AddTransient<IValidator<RequestAnswerPracticeQuestionViewModel>, AnswerPracticeQuestionValidator>();
        services.AddTransient<IValidator<RequestSetProjectViewModel>, RequestSetProjectValidator>();
        
        return services;
    }
}