using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using VacationRental.Api.Models;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddMvcCore().AddApiExplorer();
    builder.Services.AddSwaggerGen(opts => opts.SwaggerDoc("v1", new OpenApiInfo { Title = "Vacation rental information", Version = "v1" }));
    builder.Services.AddSingleton<IDictionary<int, Rental>>(new Dictionary<int, Rental>());
    builder.Services.AddSingleton<IDictionary<int, Booking>>(new Dictionary<int, Booking>());
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();        
    }
    app.UseHttpsRedirection();
    app.UseSwagger();
    app.UseSwaggerUI(opts => opts.SwaggerEndpoint("/swagger/v1/swagger.json", "VacationRental v1"));
}

app.Run();