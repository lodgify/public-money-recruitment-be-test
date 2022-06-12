using Microsoft.Extensions.DependencyInjection;

namespace VacationRental.Api.Installers
{
    public interface IInstaller
    {
        IServiceCollection InstallServices(IServiceCollection services);
    }
}
