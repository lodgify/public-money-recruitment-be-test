using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using VacationRental.Api.Infrastructure.Contracts;
using VacationRental.Api.Infrastructure.Models;
using VacationRental.Api.Infrastructure.Repositories;

namespace VacationRental.Api.Infrastructure.Container
{
    public static class Container
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            // Inmemory data
            services.AddSingleton<IDictionary<int, RentalViewModel>>(new Dictionary<int, RentalViewModel>());
            services.AddSingleton<IDictionary<int, BookingViewModel>>(new Dictionary<int, BookingViewModel>());

            // Services for Infrastructure
            services.AddTransient<IBookingRepository, BookingRepository>();
            services.AddTransient<IRentalRepository, RentalRepository>();
        }
    }
}
