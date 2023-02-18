using Models.ViewModels;

namespace VacationRental.Api.Operations.CalendarOperations;

public interface ICalendarGetOperation
{
    Task<CalendarViewModel> ExecuteAsync(int rentalId, DateTime start, int nights);
}

