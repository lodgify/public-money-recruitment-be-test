using System;
using System.Collections.Generic;
using Application.Services;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.DAL;
using Domain.DAL.Models;
using FluentValidation;
using Infrastructure.DAL;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using VacationRental.Api.Models;
using VacationRental.Api.PipelineBehaviours;

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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerGen(opts => opts.SwaggerDoc("v1", new Info { Title = "Vacation rental information", Version = "v1" }));

            services.AddSingleton<IDictionary<int, Rental>>(new Dictionary<int, Rental>());
            services.AddSingleton<IDictionary<int, Booking>>(new Dictionary<int, Booking>());
            
            services.AddScoped<IRepository<Rental>, EntityRepository<Rental>>();
            services.AddScoped<IRepository<Booking>, EntityRepository<Booking>>();
            
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehaviour<,>));
            services.AddValidatorsFromAssembly(typeof(Application.Validators.Booking.CreateBookingCommandValidator).Assembly);
            services.AddScoped<ITimeProviderService, TimeProviderService>();
            
            services.AddAutoMapper(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
