using System;
using System.Collections.Generic;
using Logger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using VacationRental.Api.Models;
using VacationRental.Api.Models.Common;

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

            services.AddSingleton<IDictionary<int, RentalViewModel>>(new Dictionary<int, RentalViewModel>());
            services.AddSingleton<IDictionary<int, BookingViewModel>>(new Dictionary<int, BookingViewModel>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            LogHelper.logger.Info("Service is starting up");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseExceptionHandler(errorApp => 
            {
                errorApp.Run(async context => 
                {
                    ExceptionViewModel exceptionModel;
                    var exception = context.Features.Get<IExceptionHandlerFeature>();
                    LogHelper.logger.Error(exception.Error);

                    Type exceptionType = exception.Error.GetType();
                    context.Response.ContentType = "application/json";
#if DEBUG
                    string infoLink = $"{context.Request.Scheme}://{context.Request.Host.Host}:{context.Request.Host.Port}/swagger/index.html";
#else
                    string infoLink = $"https://github.com/lodgify/public-money-recruitment-be-test"
#endif
                    if (exceptionType.Name == "ApplicationException")
                    {
                        context.Response.StatusCode = 422;
                        exceptionModel = new ExceptionViewModel(exception.Error.Message, infoLink);
                    }
                    else 
                    {
                        context.Response.StatusCode = 500;
                        exceptionModel = new ExceptionViewModel("Ooops. Something went wrong.", infoLink);
                    }
                    await context.Response.WriteAsync(exceptionModel.ConvertToJson(), System.Text.Encoding.UTF8);
                });
            });
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(opts => opts.SwaggerEndpoint("/swagger/v1/swagger.json", "VacationRental v1"));

            LogHelper.logger.Info("Service has started successfully");
        }
    }
}
