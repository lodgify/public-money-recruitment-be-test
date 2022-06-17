using VacationRental.Models.Dtos;

namespace VacationRental.Services.Interfaces
{
    public interface ICalendarService
    {
        Task<CalendarDto> GetCalendarAsync(int rentalId, int nights, DateTime start);
    }
}
