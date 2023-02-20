using Models.DataModels;
using Repository.Repository;

namespace VacationRental.Api.IoC;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddSingleton<IDictionary<int, RentalDto>>(new Dictionary<int, RentalDto>())
            .AddSingleton<IDictionary<int, BookingDto>>(new Dictionary<int, BookingDto>())
            .AddSingleton<IDictionary<int, UnitDto>>(new Dictionary<int, UnitDto>())

            .AddScoped<IBookingRepository, BookingRepository>()
            .AddScoped<IRentalRepository, RentalRepository>()
            .AddScoped<IUnitRepository, UnitRepository>();
    }
}
