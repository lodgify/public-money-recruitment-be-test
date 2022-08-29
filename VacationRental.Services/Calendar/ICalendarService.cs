using System;
using VacationRental.Services.Models.Calendar;

namespace VacationRental.Services.Calendar
{
    public interface ICalendarService
    {
        CalendarDto GetCalendar(int rentalId, DateTime start, int nights);
    }
}
