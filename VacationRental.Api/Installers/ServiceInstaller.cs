using Microsoft.Extensions.DependencyInjection;
using System;
using VacationRental.Services;
using VacationRental.Services.IServices;
using VacationRental.Services.Services;

namespace VacationRental.Api.Installers
{
    public class ServiceInstaller : IInstaller
    {
        public IServiceCollection InstallServices(IServiceCollection services)
        {
            services.AddScoped<IRentalService, RentalService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<ICalenderService, CalenderService>();
            services.AddScoped<IBookingHelper, BookingHelper>();

            return services;
        }
    }
}
