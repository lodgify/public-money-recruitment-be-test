using System.Collections.Generic;
using VacationRental.Domain.Bookings;

namespace VacationRental.Application.Common.Services.BookingSearchService
{
    public interface IBookingSearchService
    {
        IEnumerable<BookingModel> GetBookingsByDay(GetBookingsByDayDTO model);
        IEnumerable<BookingModel> GetBookingsByRangeOfTime(GetBookingsByRangeOfTimeDTO model);
    }
}
