using Microsoft.Extensions.DependencyInjection;
using VacationRental.Domain.Repositories;
using VacationRental.Infrastructure.Persist;

namespace VacationRental.Infrastructure.DI.Extensions
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IRentalRepository, RentalRepository>();

            return services;
        }
    }
}
