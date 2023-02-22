using VacationRental.Api.Operations.BookingOperations;
using VacationRental.Api.Operations.CalendarOperations;
using VacationRental.Api.Operations.RentalsOperations;
using VacationRental.Api.Operations.UnitOperations;

namespace VacationRental.Api.IoC;

public static class OperationsExtensions
{
    public static IServiceCollection AddOperations(this IServiceCollection services)
    {
        return services
            .AddScoped<IBookingCreateOperation, BookingCreateOperation>()
            .AddScoped<IBookingGetOperation, BookingGetOperation>()
            .AddScoped<IBookingPreparationCheckOperation, BookingPreparationCheckOperation>()

            .AddScoped<IRentalCreateOperation, RentalCreateOperation>()
            .AddScoped<IRentalUpdateOperation, RentalUpdateOperation>()
            .AddScoped<IRentalGetOperation, RentalGetOperation>()

            .AddScoped<IUnitCreateOperation, UnitCreateOperation>()
            .AddScoped<IUnitUpdateOperation, UnitUpdateOperation>()
            .AddScoped<IUnitListGetOperation, UnitListGetOperation>()

            .AddScoped<ICalendarGetOperation, CalendarGetOperation>();
    }
}