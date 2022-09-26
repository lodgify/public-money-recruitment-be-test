using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using VacationRental.BusinessLogic.Services;
using VacationRental.BusinessLogic.Services.Interfaces;

namespace VacationRental.BusinessLogic.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionsExtensions
    {
        public static void AddBusinessLogicServices(this IServiceCollection services)
        {
            services.AddTransient<IBookingsService, BookingsService>();
            services.AddTransient<IRentalsService, RentalsService>();
            services.AddTransient<ICalendarsService, CalendarsService>();
        }
    }
}
