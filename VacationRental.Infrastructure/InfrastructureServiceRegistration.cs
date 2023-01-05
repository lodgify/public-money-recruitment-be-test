using Microsoft.Extensions.DependencyInjection;
using VacationRental.Application.Contracts.Persistence;
using VacationRental.Infrastructure.Repositories;

namespace VacationRental.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection service)
        {                        
            service.AddSingleton(typeof(IRepository<>), typeof(BaseRepository<>));
            service.AddSingleton<IBookingRepository, BookingRepository>();

            return service;
        }
    }
}
