using VacationRental.Infrastructure.DTOs;

namespace VacationRental.Infrastructure.Services.Interfaces
{
    public interface ICalendarService
    {
        CalendarDTO GetCalendar(int rentalId, DateTime start, int nights);
    }
}
