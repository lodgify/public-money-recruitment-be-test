using Microsoft.Extensions.DependencyInjection;
using VacationRental.Services;
using VacationRental.Services.Interfaces;

namespace VacationRental.Infrastructure.Extensions
{
    public static class ServicesExtensions
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<ICalendarService, CalendarService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IRentalService, RentalService>();
        }
    }
}
