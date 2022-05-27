using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using VacationRental.Api.Presenter;
using VacationRental.Api.Presenter.Interfaces;
using VacationRental.Application.Handlers.Rentals;
using VacationRental.Domain.ViewModels;
using VacationRental.Persistance;
using VacationRental.Persistance.Interfaces;
using VacationRental.Persistance.Repository;

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
            services.AddMvc(x => x.EnableEndpointRouting = false);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(opts => opts.SwaggerDoc("v1", new Info { Title = "Vacation rental information", Version = "v1" }));

            services.AddSingleton<IDictionary<int, RentalViewModel>>(new Dictionary<int, RentalViewModel>());
            services.AddSingleton<IDictionary<int, BookingViewModel>>(new Dictionary<int, BookingViewModel>());
            
            services.AddMediatR(typeof(CreateRentalHandler).Assembly);
            services.AddTransient<IVacationRentalPresenter, VacationRentalPresenter>();
            services.AddScoped(typeof(IRepository<>), typeof(VacationRentalRepository<>));

            services.AddDbContext<ApplicationDbContext>((opt => opt.UseInMemoryDatabase("ApplicationDbContext")));
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
