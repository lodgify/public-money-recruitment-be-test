using MediatR;
using Microsoft.Extensions.DependencyInjection;
using VacationRental.Application.Commands.Booking;

namespace VacationRental.Api.DI
{
    public static class ApplicationServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            var applicationAssembly = typeof(BookingCommandRequest).Assembly;
            services.AddMediatR(applicationAssembly);
        }
    }
}
