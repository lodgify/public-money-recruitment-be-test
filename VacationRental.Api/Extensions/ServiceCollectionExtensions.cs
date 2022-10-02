using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using VacationRental.Domain.VacationRental.Interfaces;
using VacationRental.Domain.VacationRental.Interfaces.Repositories;
using VacationRental.Domain.VacationRental.Service;
using VacationRental.Infra;
using VacationRental.Infra.Repoitory;

namespace VacationRental.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBusinessServices(this IServiceCollection services)
        {
            services.AddSingleton<IDictionary<DateTime, int>>(new Dictionary<DateTime, int>());
            services.AddScoped<ICalendarService, CalendarService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IRentalsService, RentalsService>();
            services.AddTransient<IRentalsRepository, RentalsRepository>();
            services.AddTransient<IBookingRepository, BookingRepository>();
           
            services.AddDbContext<DatabaseInMemoryContext>(opt => opt.UseInMemoryDatabase(databaseName: "BookingDb"));
            services.AddMvc();
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Vacation rental information",
                    Version = "v1"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                s.IncludeXmlComments(xmlPath);
                s.IncludeXmlComments(string.Format(@"{0}\VacationRental.Domain.xml", AppContext.BaseDirectory));
            });

        }
    }
    
}
