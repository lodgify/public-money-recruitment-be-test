using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Text.Json.Serialization;
using VacationRental.DataAccess.Contexts;
using VacationRental.Infrastructure.Extensions;
using VacationRental.Infrastructure.Middlewares;
using VacationRental.Infrastructure.Validators;
using VacationRental.Models.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AutoMapper;
using VacationRental.Infrastructure.Profiles;

const string defaultConnectionParamName = "DefaultConnection";

const string swaggerTitle = "Vacation Rental";
const string swaggerVersion = "v1";
const string swaggerUrl = "/swagger/v1/swagger.json";
const string swaggerName = "Vacation Rental v1";
const string swaggerMediaType = "application/json";

const string healthzPath = "/healthz";
const string healthzApiPath = "/api/healthz";
const string healthzReadyPath = "/healthz/ready";
const string healthzLivePath = "/healthz/live";
const string healthzServiceUrlParamName = "HealthCheck:ServiceUrl";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(configure => {
    configure.Filters.Add(new ProducesAttribute(swaggerMediaType));
    configure.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorInfoDto), StatusCodes.Status400BadRequest));
    configure.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status401Unauthorized));
    configure.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status403Forbidden));
    configure.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorInfoDto), StatusCodes.Status404NotFound));
    configure.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorInfoDto), StatusCodes.Status500InternalServerError));
}).AddJsonOptions(configure => {
    configure.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Configure IdentityServer4
builder.Services.ConfigureIdentityServer(builder.Configuration);

// Configure Api Versioning
builder.Services.AddApiVersioning();

// Configure AutoMapper
builder.Services.AddAutoMapper(configure => {
    configure.AddProfile<BookingProfile>();
    configure.AddProfile<RentalProfile>();
});

// Configure FluentValidation
builder.Services.AddFluentValidation(configure => configure.RegisterValidatorsFromAssemblyContaining<BookingParametersValidator>());

// Configure MSSQL [VirtualRentelDB]
var defaultConnection = builder.Configuration.GetConnectionString(defaultConnectionParamName);
builder.Services.AddDbContext<VacationRentalDbContext>(options => options.UseSqlServer(defaultConnection,
                                                       options => options.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)));

// Configure DI
builder.Services.ConfigureRepositories();
builder.Services.ConfigureServices();

// Configure Health Checks
builder.Services.AddHealthChecks()
                .AddDbContextCheck<VacationRentalDbContext>()
                .AddUrlGroup(new Uri($"{builder.Configuration[healthzServiceUrlParamName]}{healthzApiPath}"));

// Configure ApplicationInsights
builder.Services.AddApplicationInsightsTelemetry();

// Configure Swagger
builder.Services.AddSwaggerGen(configure => {
    configure.SwaggerDoc(swaggerVersion, new OpenApiInfo
    {
        Title = swaggerTitle,
        Version = swaggerVersion
    });

    configure.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    configure.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id=JwtBearerDefaults.AuthenticationScheme
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();

app.UseIdentityServer();

app.UseAuthentication();

app.UseAuthorization();

app.MapHealthChecks(healthzPath);

app.MapHealthChecks(healthzReadyPath, new HealthCheckOptions
{
    Predicate = healthCheck => healthCheck.Tags.Contains("ready")
});

app.MapHealthChecks(healthzLivePath, new HealthCheckOptions
{
    Predicate = _ => false
});

app.Map(healthzApiPath, configuration => configuration.Use(async (context, next) =>
{
    await context.Response.WriteAsync(HealthStatus.Healthy.ToString());
    await next(context);
}));

app.UseSwagger();

app.UseSwaggerUI(opts => opts.SwaggerEndpoint(swaggerUrl, swaggerName));

app.MapControllers();

app.Run();
