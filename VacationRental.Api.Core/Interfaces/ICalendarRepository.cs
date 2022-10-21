using System;
using VacationRental.Api.Core.Models;

namespace VacationRental.Api.Core.Interfaces
{
    public interface ICalendarRepository
    {
        CalendarViewModel GetRentalCalendar(int rentalId, DateTime start, int nights);
    }
}
