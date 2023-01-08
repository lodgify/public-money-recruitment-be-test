using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using VacationRental.Application.Contracts.Pipeline;
using VacationRental.Application.Features.Bookings.Commands.CreateBooking;
using VacationRental.Application.Features.Bookings.Queries.GetBooking;
using VacationRental.Application.Features.Calendars.Queries.GetRentalCalendar;
using VacationRental.Application.Features.Rentals.Commands.CreateRental;
using VacationRental.Application.Features.Rentals.Queries.GetRental;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Messages.Bookings;
using VacationRental.Domain.Messages.Calendars;
using VacationRental.Domain.Messages.Rentals;

namespace VacationRental.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddScoped(typeof(IQueryHandler<GetBookingQuery, BookingDto>), typeof(GetBookingQueryHandler));
            services.AddScoped(typeof(IQueryHandler<GetRentalQuery, RentalDto>), typeof(GetRentalQueryHandler));
            services.AddScoped(typeof(IQueryHandler<GetRentalCalendarQuery, CalendarDto>), typeof(GetRentalCalendarQueryHandler));

            services.AddScoped(typeof(ICommandHandler<CreateRentalCommand, ResourceId>), typeof(CreateRentalCommandHandler));
            services.AddScoped(typeof(ICommandHandler<CreateBookingCommand, ResourceId>), typeof(CreateBookingCommandHandler));            

            return services;
        }
    }
}
