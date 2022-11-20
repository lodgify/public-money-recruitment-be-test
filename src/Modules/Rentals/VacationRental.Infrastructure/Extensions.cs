using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using VacationRental.Core.Repositories;
using VacationRental.Infrastructure.EF;
using VacationRental.Infrastructure.EF.Repositories;

[assembly: InternalsVisibleTo("VacationRental.Api")]
[assembly: InternalsVisibleTo("VacationRental.Tests.Integration")]
[assembly: InternalsVisibleTo("VacationRental.Tests.EndToEnd")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace VacationRental.Infrastructure
{
    internal static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<RentalsDbContext>();
            services.AddScoped<IRentalRepository, RentalRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();

            return services;
        }
    }
}
