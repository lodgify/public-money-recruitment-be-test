using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using VacationRental.Application.Common.Services;

namespace VacationRental.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddScoped<IBookingSearchService, BookingSearchService>();
            return services;
        }
    }
}