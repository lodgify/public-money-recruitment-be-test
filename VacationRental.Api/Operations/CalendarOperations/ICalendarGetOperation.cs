using Models.ViewModels.Calendar;

namespace VacationRental.Api.Operations.CalendarOperations;

public interface ICalendarGetOperation
{
    Task<CalendarViewModel> ExecuteAsync(int rentalId, DateTime start, int nights);
}

