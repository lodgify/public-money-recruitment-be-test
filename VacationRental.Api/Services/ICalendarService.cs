using System;
using System.Dynamic;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services;

public interface ICalendarService
{
    CalendarViewModel Get(int rentalId, DateTime start, int nights);
}