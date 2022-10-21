using LanguageExt;
using VacationRental.Api.Core.Models;
using VacationRental.Api.Models;

namespace VacationRental.Api.Interfaces
{
    public interface ICalendarService
    {
        Result<CalendarViewModel> GetCalendar(CalendarRequestModel request);
    }
}
