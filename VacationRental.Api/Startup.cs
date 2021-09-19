using System.Collections.Generic;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using VacationRental.Api.Models;
using VacationRental.Api.Validation.ValidationUnit;

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
            services.AddMvc(options =>
                {
                    options.EnableEndpointRouting = false;
                    options.Filters.Add<ValidationFilter>();
                })
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RentalValidator>())
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<BookingViewModel>())
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CalendarViewModel>())
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddSwaggerGen(opts => opts.SwaggerDoc("v1", new OpenApiInfo { Title = "Vacation rental information", Version = "v1" }));

            services.AddSingleton<IDictionary<int, RentalViewModel>>(new Dictionary<int, RentalViewModel>());
            services.AddSingleton<IDictionary<int, BookingViewModel>>(new Dictionary<int, BookingViewModel>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(opts => opts.SwaggerEndpoint("/swagger/v1/swagger.json", "VacationRental v1"));
        }
    }
}
