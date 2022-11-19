using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using VacationRental.Core.Repositories;
using VacationRental.Infrastructure.Repositories;

[assembly: InternalsVisibleTo("VacationRental.Api")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace VacationRental.Infrastructure
{
    internal static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IRentalRepository, RentalRepository>();
            services.AddSingleton<IBookingRepository, BookingRepository>();

            return services;
        }
    }
}
