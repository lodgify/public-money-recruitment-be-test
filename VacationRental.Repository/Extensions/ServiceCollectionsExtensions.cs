using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using VacationRental.Repository.Entities;
using VacationRental.Repository.Repositories;
using VacationRental.Repository.Repositories.Interfaces;

namespace VacationRental.Repository.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionsExtensions
    {
        public static void AddRepositoryServices(this IServiceCollection services)
        {
            services.AddSingleton<IDictionary<int, RentalEntity>>(new Dictionary<int, RentalEntity>());
            services.AddSingleton<IDictionary<int, BookingEntity>>(new Dictionary<int, BookingEntity>());
            services.AddTransient<IBookingsRepository, BookingsRepository>();
            services.AddTransient<IRentalRepository, RentalRepository>();
        }
    }
}
