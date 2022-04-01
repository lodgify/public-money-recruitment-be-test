using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using VacationRental.Application.Common.Services.BookingSearchService;
using VacationRental.Application.Common.Services.ReCalculateBookingsService;

namespace VacationRental.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddScoped<IBookingSearchService, BookingSearchService>();
            services.AddScoped<IValidateRentalModificationService, ValidateRentalModificationService>();
            return services;
        }
    }
}
