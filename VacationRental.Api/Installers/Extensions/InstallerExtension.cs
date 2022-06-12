using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace VacationRental.Api.Installers.Extensions
{
    public static class InstallerExtension
    {
        public static IServiceCollection InstallServicesInAssembly(this IServiceCollection services)
        {
            var installers = typeof(Startup).Assembly.ExportedTypes.Where(x => typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
               .Select(Activator.CreateInstance).Cast<IInstaller>().ToList();

            installers.ForEach(installer => installer.InstallServices(services));

            return services;
        }
    }
}
