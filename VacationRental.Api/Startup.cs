using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using VacationalRental.Domain.Business;
using VacationalRental.Domain.Interfaces.Repositories;
using VacationalRental.Domain.Interfaces.Services;
using VacationalRental.Domain.Services;
using VacationalRental.Infrastructure.DbContexts;
using VacationalRental.Infrastructure.Repositories;

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
            services.AddDbContext<VacationRentalDbContext>(options => options.UseInMemoryDatabase("VacationRentalDb"));

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Vacation rental information", Version = "v1" });
            });

            //services.AddSwaggerGen(opts => opts.SwaggerDoc("v1", new Info { Title = "Vacation rental information", Version = "v1" }));

            services.AddScoped<IBookingsRepository, BookingsRepository>();
            services.AddScoped<IRentalsRepository, RentalsRepository>();

            services.AddTransient<IBookingService, BookingService>();
            services.AddTransient<ICalendarService, CalendarService>();
            services.AddTransient<IRentalService, RentalService>();

            //services.AddSingleton<IDictionary<int, RentalViewModel>>(new Dictionary<int, RentalViewModel>());
            //services.AddSingleton<IDictionary<int, BookingViewModel>>(new Dictionary<int, BookingViewModel>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == Configuration["ASPNETCORE_ENVIRONMENT"])
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(opts => opts.SwaggerEndpoint("/swagger/v1/swagger.json", "VacationRental v1"));
        }
    }
}
