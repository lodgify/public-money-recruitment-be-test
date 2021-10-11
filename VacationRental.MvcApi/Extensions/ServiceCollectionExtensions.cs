using Microsoft.Extensions.DependencyInjection;
using VacationRental.Booking.Services;
using VacationRental.Domain.Interfaces;
using VacationRental.Infrastructure.Data;
using VacationRental.Infrastructure.Data.Repositories;
using VacationRental.Rental.Services;

namespace VacationRental.MvcAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            // .AddSingleton<IRentalRepository, RentalRepository>();
        }

        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            return services
                .AddScoped<IUnitOfWork, UnitOfWork>();
        }


        public static IServiceCollection AddBusinessServices(this IServiceCollection services
           )
        {
            return services
                .AddBookingModule()
                .AddRentalModule();
        }
    }
}