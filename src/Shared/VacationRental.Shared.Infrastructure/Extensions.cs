using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using VacationRental.Shared.Infrastructure.Dispatchers;
using VacationRental.Shared.Infrastructure.Exceptions;

namespace VacationRental.Shared.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddCommonInfrastructure(this IServiceCollection services)
        {
            services.AddErrorHandling();
            services.AddDispatchers();

            return services;
        }

        public static IApplicationBuilder UseCommonInfrastucture(this IApplicationBuilder app)
        {
            app.UseErrorHandling();

            return app;
        }
    }
}
