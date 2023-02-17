using Models.ViewModels;

namespace VacationRental.Api.IoC
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IDictionary<int, RentalViewModel>>(new Dictionary<int, RentalViewModel>())
                .AddSingleton<IDictionary<int, BookingViewModel>>(new Dictionary<int, BookingViewModel>());
        }
    }
}