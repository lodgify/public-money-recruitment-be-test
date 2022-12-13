using System;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public interface ICalendarService
    {
        CalendarViewModel GetCalendar(int rentalId, DateTime start, int nights);
    }
}
