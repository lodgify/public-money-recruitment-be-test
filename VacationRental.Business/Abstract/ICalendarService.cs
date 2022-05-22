using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Domain.DTOs;
using VacationRental.Domain.Entities;

namespace VacationRental.Business.Abstract
{
    public interface ICalendarService
    {
        List<List<KeyValuePair<int, Booking>>> GetBookings(CalendarDto dto);
    }
}
