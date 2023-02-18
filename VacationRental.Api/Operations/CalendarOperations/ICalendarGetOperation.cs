using Models.ViewModels;

namespace VacationRental.Api.Operations.CalendarOperations;

public interface ICalendarGetOperation
{
    CalendarViewModel ExecuteAsync(int rentalId, DateTime start, int nights);
}

