using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using VacationRental.Application;
using VacationRental.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices();
    builder.Services.AddMvcCore().AddApiExplorer();
    builder.Services.AddSwaggerGen(opts => opts.SwaggerDoc("v1", new OpenApiInfo { Title = "Vacation rental information", Version = "v1" }));
    builder.Services.AddCors(opt =>
    {
        opt.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
    });
}

var app = builder.Build();
{
    app.UseCors("CorsPolicy");
    app.MapControllers();

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();        
    }
    app.UseHttpsRedirection();
    app.UseSwagger();
    app.UseSwaggerUI(opts => opts.SwaggerEndpoint("/swagger/v1/swagger.json", "VacationRental v1"));
}

app.Run();


public partial class Program { }