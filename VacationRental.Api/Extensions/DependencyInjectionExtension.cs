using Microsoft.Extensions.DependencyInjection;
using VacationRental.Application.Midlewares.Booking;
using VacationRental.Application.Midlewares.Calendar;
using VacationRental.Application.Midlewares.Rental;
using VacationRental.Infra;
using VacationRental.Infra.Repositories;
using VacationRental.Infra.Repositories.Interfaces;

namespace VacationRental.Api.Extensions
{
	public static class DependencyInjectionExtension
	{
		public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
		{
			services.AddTransient<VacationRentalContext>();
			services.AddScoped<IRentalRepository, RentalRepository>();
			services.AddScoped<IRentalMiddleware, RentalMiddleware>();
			services.AddScoped<IBookingRepository, BookingRepository>();
			services.AddScoped<IBookingMiddleware, BookingMidleware>();
			services.AddScoped<ICalendarMiddleware, CalendarMiddleware>();
			return services;
		}
	}
}
