using Microsoft.Extensions.DependencyInjection;

namespace VR.Infrastructure.Modules
{
    public interface IModule
    {
        void ConfigureServices(IServiceCollection services);
    }
}
