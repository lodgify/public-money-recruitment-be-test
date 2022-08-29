using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using VacationRental.Api.Infrastructure;
using VacationRental.Core.Data;
using VacationRental.Data;
using VacationRental.Services.Bookings;
using VacationRental.Services.Calendar;
using VacationRental.Services.Rentals;

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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            AddSwagger(services);

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                });

            services.AddCors(options =>
            {
                options.AddPolicy("AllRequests", builder =>
                {
                    builder.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });
            });

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = new MediaTypeApiVersionReader();
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
            });

            services.AddAutoMapper(options =>
                {
                    options.AddProfile<BookingProfile>();
                    options.AddProfile<RentalProfile>();
                });

            RegisterRepositories(services);

            RegisterServices(services);
        }

        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                var version = "v1";

                options.SwaggerDoc(version, new OpenApiInfo
                {
                    Title = "Vacation Rental API",
                    Version = version,
                    Description = $"Vacation rental information API {version.ToUpperInvariant()}",
                });
            });
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IBookingsService, BookingsService>();
            services.AddScoped<IRentalService, RentalService>();
            services.AddScoped<ICalendarService, CalendarService>();
        }

        private void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<DbContext, VacationRentalObjectContext>();

            services.AddDbContext<VacationRentalObjectContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryVacationRentalDB");
            });



            //services.AddDbContext<VacationRentalObjectContext>(options =>
            //{
            //    var connectionString = Configuration.GetConnectionString("DefaultConnection");
            //    options.UseSqlServer(connectionString, builder =>
            //    {
            //        builder.EnableRetryOnFailure(
            //            maxRetryCount: 3,
            //            maxRetryDelay: TimeSpan.FromSeconds(30),
            //            errorNumbersToAdd: null);
            //    });
            //});

            services.AddScoped(typeof(IRepository<,>), typeof(EfRepository<,>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(opts => opts.SwaggerEndpoint("/swagger/v1/swagger.json", "VacationRental v1"));

            app.UseCors("AllRequests");

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
