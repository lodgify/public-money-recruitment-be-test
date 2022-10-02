using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using VacationRental.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions();
var configuration = (IConfiguration)builder.Configuration;
builder.Services.AddLogging();

builder.Services.AddBusinessServices();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
});

builder.Services.AddControllers();

builder.Services.ConfigureSwagger();

var app = builder.Build();
app.UseCors();
app.ConfigureExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();
if (app.Environment.IsEnvironment("Testing") || app.Environment.IsEnvironment("Staging") || app.Environment.IsDevelopment())
{
    var basePath = "/VacationRental v1";
    app.UseSwagger(c =>
    {
        c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
        {
            if (httpReq.Host.Host == "localhost")
                return;
            swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"https://{httpReq.Host.Value}{basePath}" } };
        });
    });
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

public partial class Program { }
