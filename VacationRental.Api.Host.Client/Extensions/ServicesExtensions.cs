using Microsoft.Extensions.DependencyInjection;
using VacationRental.Api.Host.Client.Clients;
using VacationRental.Api.Host.Client.Interfaces;

namespace VacationRental.Api.Host.Client.Extensions
{
    public static class ServicesExtensions
    {
        public static void ConfigureVacationRentalClient(this IServiceCollection services)
        {
            services.AddScoped<IVacationRentalClient, VacationRentalClient>();
        }
    }
}
