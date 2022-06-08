using Microsoft.Extensions.DependencyInjection;
using VacationRental.Services;
using VacationRental.Services.Interfaces;

namespace VacationRental.Infrastructure.Extensions
{
    public static class ServicesExtension
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<ICalendarService, CalendarService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IRentalService, RentalService>();
        }
    }
}
