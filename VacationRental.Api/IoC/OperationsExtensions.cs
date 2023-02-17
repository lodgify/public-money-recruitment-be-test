using VacationRental.Api.Operations.BookingOperations;
using VacationRental.Api.Operations.CalendarOperations;
using VacationRental.Api.Operations.RentalsOperations;

namespace VacationRental.Api.IoC
{
    public static class OperationsExtensions
    {
        public static IServiceCollection AddOperations(this IServiceCollection services)
        {
            return services
                .AddScoped<IBookingCreateOperation, BookingCreateOperation>()
                .AddScoped<IBookingGetOperation, BookingGetOperation>()

                .AddScoped<ICalendarGetOperation, CalendarGetOperation>()

                .AddScoped<IRentalGetOperation, RentalGetOperation>()
                .AddScoped<IRentalCreateOperation, RentalCreateOperation>();
        }
    }
}