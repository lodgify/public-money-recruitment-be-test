using Microsoft.Extensions.DependencyInjection;
using VacationRental.Api.Core.Interfaces;
using VacationRental.Api.Core.Repositories;

namespace VacationRental.Api.Core.Containers
{
    public static class CoreContainer
    {
        public static void AddCoreServices(this IServiceCollection services)
        {
            services.AddTransient<IBookingService, BookingService>();
            services.AddTransient<ICalendarService, CalendarService>();
            services.AddTransient<IRentalService, RentalService>();
        }
    }
}
