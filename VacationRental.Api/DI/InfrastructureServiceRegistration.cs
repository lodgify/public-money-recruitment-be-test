using Microsoft.Extensions.DependencyInjection;
using VacationRental.Domain.Repositories;
using VacationRental.Domain.Repositories.ReadOnly;
using VacationRental.Infrastructure.Persist.PersistModels;
using VacationRental.Infrastructure.Persist.Repositories;
using VacationRental.Infrastructure.Persist.Storage;

namespace VacationRental.Api.DI
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddSingleton<IInMemoryDataStorage<BookingDataModel>>(_ =>
                new InMemoryDataStorage<BookingDataModel>(model => model.Id));

            services.AddSingleton<IInMemoryDataStorage<RentalDataModel>>(_ =>
                new InMemoryDataStorage<RentalDataModel>(model => model.Id));

            services.AddScoped<IBookingReadOnlyRepository, BookingRepository>();
            services.AddScoped<IRentalReadOnlyRepository, RentalRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IRentalRepository, RentalRepository>();

            return services;
        }
    }
}
