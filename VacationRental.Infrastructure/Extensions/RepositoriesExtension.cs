using Microsoft.Extensions.DependencyInjection;
using VacationRental.DataAccess.Interfaces;
using VacationRental.DataAccess.Repositories;

namespace VacationRental.Infrastructure.Extensions
{
    public static class RepositoriesExtension
    {
        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        }
    }
}
