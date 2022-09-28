using System;
using VacationRental.BusinessObjects;

namespace VacationRental.BusinessLogic.Services.Interfaces
{
    public interface ICalendarsService
    {
        Calendar GetCalendar(int rentalId, DateTime start, int nights);
    }
}
