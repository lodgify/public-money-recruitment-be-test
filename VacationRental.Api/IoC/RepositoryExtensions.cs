using Models.ViewModels;
using VacationRental.Api.Repository;

namespace VacationRental.Api.IoC;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddSingleton<IDictionary<int, RentalViewModel>>(new Dictionary<int, RentalViewModel>())
            .AddSingleton<IDictionary<int, BookingViewModel>>(new Dictionary<int, BookingViewModel>())

            .AddScoped<IBookingRepository, BookingRepository>()
            .AddScoped<IRentalRepository, RentalRepository>();
    }
}
