using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using VR.Infrastructure.Mapping;
using VR.Infrastructure.Mapping.Interfaces;
using VR.Infrastructure.Modules;

namespace VR.Infrastructure
{
    public class InfrastructureModule : IModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.Contains("VR")
                                 || a.FullName.Contains("VacationRental"));

            ObjectMapper.Global.Scan(assemblies);

            services.AddSingleton((IObjectMapper)ObjectMapper.Global);
        }
    }
}
