using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Text.Json.Serialization;
using VacationRental.DataAccess;
using VacationRental.Infrastructure.Extensions;
using VacationRental.Infrastructure.Middlewares;
using VacationRental.Infrastructure.Validators;
using VacationRental.Models.Dtos;

const string defaultConnectionParamName = "DefaultConnection";
//const string identityServerConnectionParamName = "IdentityServerConnection";

const string swaggerTitle = "Vacation Rental";
const string swaggerVersion = "v1";
const string swaggerUrl = "/swagger/v1/swagger.json";
const string swaggerName = "VacationRental v1";
const string swaggerMediaType = "application/json";

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
builder.Services.AddApiVersioning();

// Configer AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Configure FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining(typeof(BookingParametersValidator));

// Configure Virtual Rentel DB
var defaultConnection = builder.Configuration.GetConnectionString(defaultConnectionParamName);
builder.Services.AddDbContext<VacationRentalDbContext>(options => options.UseSqlServer(defaultConnection,
                                                       options => options.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)));

// Configure DI
builder.Services.ConfigureRepositories();
builder.Services.ConfigureServices();

// Configure ApplicationInsights
builder.Services.AddApplicationInsightsTelemetry();

// Configure Swagger
builder.Services.AddSwaggerGen(configure => configure.SwaggerDoc(swaggerVersion, new OpenApiInfo { 
    Title = swaggerTitle, 
    Version = swaggerVersion 
}));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Configure Error Handling
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

// Configure Swagger
app.UseSwagger();

app.UseSwaggerUI(opts => opts.SwaggerEndpoint(swaggerUrl, swaggerName));

app.MapControllers();

app.Run();
