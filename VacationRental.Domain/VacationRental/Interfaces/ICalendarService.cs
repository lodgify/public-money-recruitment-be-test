using VacationRental.Domain.VacationRental.Models;

namespace VacationRental.Domain.VacationRental.Interfaces
{
    public interface ICalendarService
    {
        Task<CalendarViewModel> Get(int rentalId, DateTime start, int nights);
    }
}
