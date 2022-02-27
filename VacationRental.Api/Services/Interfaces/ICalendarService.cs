using VacationRental.Api.Models;

namespace VacationRental.Api.Services.Interfaces
{
    public interface ICalendarService
    {
        CalendarViewModel Get(CalendarBindingModel model);
    }
}
