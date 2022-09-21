using Microsoft.Extensions.DependencyInjection;
using VR.Application.Resolvers;
using VR.Infrastructure.Modules;

namespace VR.Application
{
    public class ApplicationModule : IModule
    {Test
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IBookingConflictResolver, BookingConflictResolver>();
        }
    }
}
