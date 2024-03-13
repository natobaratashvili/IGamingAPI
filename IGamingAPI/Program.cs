using IGaming.API.Middlewares;
using IGaming.Core.DatabaseAccessHelpers;
using IGaming.Core.UsersManagement.Repositories.Implementation;
using IGaming.Core.UsersManagement.Repositories.Interfaces;
using IGaming.Core.UsersManagement.Services.Implementation;
using IGaming.Core.UsersManagement.Services.Interfaces;
using IGaming.Infrastructure.Security.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;
using IGaming.Core.UsersManagement.Validators;
using Hellang.Middleware.ProblemDetails;
using IGaming.API.Filters;
using IGaming.Core.Bets.Services.Interfaces;
using IGaming.Core.Bets.Repositories.Interfaces;
using IGaming.Core.Bets.Repositories.Implementation;
using IGaming.Core.Bets.Services.Implementation;
using IGaming.Core.Database;

;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "IGamingApi", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
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
                    Array.Empty<string>()
                }
            });
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});


builder.Services.AddValidatorsFromAssemblyContaining<UserRegistrationRequestValidator>();

builder.Services.AddJwtProvider( settings => builder.Configuration.GetSection("JwtSettings").Bind(settings));
builder.Services.AddHasher();
builder.Services.AddDbConnectionProvider(settings => builder.Configuration.GetSection("DbConfig").Bind(settings));
builder.Services.AddScoped<IUserManagementService, UserManagementService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBetsManagementService, BetsManagementService>();
builder.Services.AddScoped<IBetsRepository, BetsRepository>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddSingleton<IDbWrapper, DbWrapper>();

var app = builder.Build();
//app.UseProblemDetails();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseApiVersioning();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();
