using System.Text;
using Api.Filter;
using Application;
using Application.IRepositories;
using Domain.Entities.UserAgg;
using FluentValidation.AspNetCore;
using Infrastructure.IOC.IdentityContextConfigs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence.Contexts;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.RegisterAllApplicationServices();
services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters().RegisterFluentValidationServices();
services.AddControllers();

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange:
        true)
    .Build();

services.AddMvc(opt =>
{
    opt.Filters.Add<HttpGlobalExceptionFilter>();
    opt.Filters.Add<GlobalFilter>();
    opt.Filters.Add<ValidationFilter>();
}).AddControllersAsServices();

services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        configurePolicy => configurePolicy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithHeaders()
            .WithExposedHeaders("AccessToken", "RefreshToken"));
});

//services.AddAutoMapper(AutoMapperConfig.RegisterMappings());

services
    .AddIdentityContext<ApplicationDataBaseContext, ApplicationUser, ApplicationRole, int, IUnitOfWork>(
        builder.Configuration,
        builder.Configuration.GetConnectionString("Application"));

services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;   // Require at least one numeric character
    options.Password.RequiredLength = 8;    // Minimum 8 characters
    options.Password.RequireNonAlphanumeric = false;  // You can set this true if you want a special character
    options.Password.RequireUppercase = true;  // Require at least one uppercase letter
    options.Password.RequireLowercase = true;  // Require at least one lowercase letter
    //options.Password.RequiredUniqueChars = 1;  // 
});

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtToken:Issuer"],
            ValidAudience = builder.Configuration["JwtToken:Audience"],
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtToken:Secret"])),
            ClockSkew = TimeSpan.Zero
        };
    });

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

// services.AddEntityFrameworkSqlServer().AddDbContext<ApplicationDataBaseContext>(option =>
//     option.UseSqlServer(builder.Configuration.GetConnectionString("Application")));

// Using PostgreSQL configuration
services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationDataBaseContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Application")));



builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "DKDRustaBazaar Web Portal APIs",
        Description = "DKDRustaBazaar Core Web API"
    });

    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Bearer Token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

app.UseRouting();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DKDRustaBazaar Web Portal APIs v1");
        c.RoutePrefix = string.Empty;
    });
//}
    app.UseHttpsRedirection();
    app.MapControllers();

    app.UseCors("CorsPolicy");
    app.UseAuthentication();
    app.UseAuthorization();

    app.Run();
}