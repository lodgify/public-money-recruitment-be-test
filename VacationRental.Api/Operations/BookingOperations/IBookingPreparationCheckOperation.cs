using Models.DataModels;

namespace VacationRental.Api.Operations.BookingOperations;

public interface IBookingPreparationCheckOperation
{
    Task<IEnumerable<BookingDto>> ExecuteAsync(int rentalId, int preparationTimeInDays, DateTime orderStartDate);
}
