using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using VacationRental.Application.Aspects;
using VacationRental.Application.Commands.Booking;
using VacationRental.Application.Services;
using VacationRental.Domain.Services;

namespace VacationRental.Api.DI
{
    public static class ApplicationServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            var applicationAssembly = typeof(BookingRequest).Assembly;
            services.AddMediatR(applicationAssembly);
            services.AddValidatorsFromAssembly(applicationAssembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ErrorHandlerAspect<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationAspect<,>));

            services.AddScoped<IRentalUpdatedEventHandler, RentalUpdatedEventHandler>();
        }
    }
}
