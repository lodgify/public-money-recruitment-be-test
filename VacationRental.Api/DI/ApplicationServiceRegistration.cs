using MediatR;
using Microsoft.Extensions.DependencyInjection;
using VacationRental.Application.Aspects;
using VacationRental.Application.Commands.Booking;

namespace VacationRental.Api.DI
{
    public static class ApplicationServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            var applicationAssembly = typeof(BookingRequest).Assembly;
            services.AddMediatR(applicationAssembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ErrorHandlerAspect<,>));
        }
    }
}
