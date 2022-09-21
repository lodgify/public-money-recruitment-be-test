using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using VacationRental.Api.Models.Mapping;
using VacationRental.Data.Repositories;
using VacationRental.Data.Repositories.Abstractions;
using VacationRental.Middleware.ExceptionHandling;
using VacationRental.Services;
using VacationRental.Services.Abstractions;
using VacationRental.Services.Dto.Mapping;

namespace VacationRental.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc();

        services.AddSwaggerGen(
           options => {
               options
                   .SwaggerDoc("v1", new OpenApiInfo { Title = "Vacation rental information", Version = "v1" });

               var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
               options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
           }
        );

        services.AddSingleton<IBookingRepository, BookingRepository>();
        services.AddSingleton<IRentalRepository, RentalRepository>();
        services.AddTransient<IRentalService, RentalService>();
        services.AddTransient<IBookingService, BookingService>();
        services.AddTransient<ICalendarService, CalendarService>();
        
        services.AddAutoMapper(typeof(ViewModelMappingProfile), typeof(DtoMappingProfile));
        services.AddLogging(
            loggingBuilder => loggingBuilder
                .SetMinimumLevel(LogLevel.Trace)
                .AddDebug()
        );
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();
        app.UseExceptionHandling();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI(opts => {
            opts.SwaggerEndpoint("/swagger/v1/swagger.json", "VacationRental v1");
            opts.RoutePrefix = string.Empty;
        });
    }
}