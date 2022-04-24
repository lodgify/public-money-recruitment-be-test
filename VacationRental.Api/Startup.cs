using FluentValidation.AspNetCore;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using VacationRental.Api.Models;
using VacationRental.Data;
using VacationRental.Domain.Bookings;
using VacationRental.Domain.Rentals;
using VacationRental.Infrastructure.DTOs;
using VacationRental.Infrastructure.Services;
using VacationRental.Infrastructure.Services.Interfaces;

namespace VacationRental.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMvc().AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<RentalCreateInputDTOValidator>();
                fv.AutomaticValidationEnabled = true;
            });

            services.AddSwaggerGen(opts => opts.SwaggerDoc("v1", new OpenApiInfo { Title = "Vacation rental information", Version = "v1" }));

            services.AddSingleton<IDictionary<int, RentalViewModel>>(new Dictionary<int, RentalViewModel>());
            services.AddSingleton<IDictionary<int, BookingViewModel>>(new Dictionary<int, BookingViewModel>());


            //Register Mapster
            TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetAssembly(typeof(RentalCreateInputDTOMapping)) ??
                          throw new FileLoadException(nameof(RentalCreateInputDTOMapping) +
                                                      ".cs file not found."));
            TypeAdapterConfig.GlobalSettings.AllowImplicitDestinationInheritance = true;
            TypeAdapterConfig.GlobalSettings.AllowImplicitSourceInheritance = true;


            services.AddSingleton<IEntityRepository<Booking>>(new EntityRepository<Booking>(new Dictionary<int, Booking>()));
            services.AddSingleton<IEntityRepository<Rental>>(new EntityRepository<Rental>(new Dictionary<int, Rental>()));

            services.AddScoped<IRentalService, RentalService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<ICalendarService, CalendarService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(opts => opts.SwaggerEndpoint("/swagger/v1/swagger.json", "VacationRental v1"));
        }
    }
}
