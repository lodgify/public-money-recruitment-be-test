using System;
using System.Collections.Generic;

namespace VacationRental.Booking.Domain.Interfaces
{
    public interface ICalendar
    {
        int RentalId { get; set; }
        IList<ICalendarDay> Dates { get; set; }
    }

    public interface ICalendarDay
    {
        DateTime Date { get; set; }
        IList<ICalendarBooking> Bookings { get; set; }
        IList<IBookingUnit> PreparationTimes { get; set; }
    }

    public interface ICalendarBooking : IBookingId, IBookingUnit
    {

    }
}
