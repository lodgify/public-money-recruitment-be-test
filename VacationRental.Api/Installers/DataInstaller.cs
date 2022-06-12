using Microsoft.Extensions.DependencyInjection;
using VacationRental.Data.IRepository;
using VacationRental.Data.Repository;
using VacationRental.Data.Store;

namespace VacationRental.Api.Installers
{
    public class DataInstaller : IInstaller
    {
        public IServiceCollection InstallServices(IServiceCollection services)
        {
            services.AddScoped<IRentalRepository, RentalRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddSingleton<IDataContext, DataContext>();

            return services;
        }
    }
}
