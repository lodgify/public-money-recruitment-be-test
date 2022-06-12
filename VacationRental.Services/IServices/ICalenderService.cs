using System;
using VacationRental.Domain.Models;

namespace VacationRental.Services.IServices
{
    public interface ICalenderService
    {
        CalendarViewModel GetVacationrental(int rentalId, DateTime start, int nights);
        CalendarViewModel Get(int rentalId, DateTime start, int nights);
    }
}
