using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using VacationRental.Api.Models;
using VacationRental.Api.Providers;
using VacationRental.Api.Services;
using VacationRental.Api.Storage;
using VacationRental.Api.Validators;

namespace VacationRental.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddValidation();
            services.AddSwaggerGen(opts =>
            {
                opts.SwaggerDoc("v1", new OpenApiInfo { Title = "Vacation rental information", Version = "v1" });
            });

            services.AddSingleton<IIdGenerator, IdGenerator>();
            services.AddSingleton<IStateManager, StateManager>();
            services.AddSingleton<IRentalService, RentalService>();
            services.AddSingleton<IBookingService, BookingService>();
            services.AddSingleton<ICalendarService, CalendarService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseSwagger();
            app.UseSwaggerUI(opts =>
            {
                opts.SwaggerEndpoint("/swagger/v1/swagger.json", "VacationRental v1");
                opts.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            app.UseEndpoints(o => o.MapControllers());
        }
    }
}
