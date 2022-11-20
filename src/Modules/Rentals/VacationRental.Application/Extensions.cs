using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using VacationRental.Application.Commands;
using VacationRental.Application.Commands.Handlers;
using VacationRental.Application.DTO;
using VacationRental.Application.Queries;
using VacationRental.Application.Queries.Handlers;
using VacationRental.Shared.Abstractions.Commands;
using VacationRental.Shared.Abstractions.Queries;

[assembly: InternalsVisibleTo("VacationRental.Api")]
[assembly: InternalsVisibleTo("VacationRental.Tests.Unit")]
[assembly: InternalsVisibleTo("VacationRental.Tests.Integration")]
[assembly: InternalsVisibleTo("VacationRental.Tests.EndToEnd")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace VacationRental.Application
{
    internal static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IQueryHandler<GetRental, RentalDto>, GetRentalHandler>();
            services.AddScoped<IQueryHandler<GetBooking, BookingDto>, GetBookingHandler>();
            services.AddScoped<IQueryHandler<GetCalendar, CalendarDto>, GetCalendarHandler>();

            services.AddScoped<ICommandHandler<AddRental, int>, AddRentalHandler>();
            services.AddScoped<ICommandHandler<AddBooking, int>, AddBookingHandler>();
            services.AddScoped<ICommandHandler<UpdateRental, int>, UpdateRentalHandler>();

            return services;
        }
    }
}
