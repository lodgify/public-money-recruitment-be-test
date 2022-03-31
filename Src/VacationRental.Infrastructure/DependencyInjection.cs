using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using VacationRental.Domain.Bookings;
using VacationRental.Domain.Rentals;
using VacationRental.Infrastructure.Persistence;

namespace VacationRental.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IRentalRepository, InMemoryRentalRepository>();
            services.AddSingleton<IBookingRepository, InMemoryBookingRepository>();
            return services;
        }
    }
}