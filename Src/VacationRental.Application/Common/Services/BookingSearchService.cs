using System.Collections.Generic;
using System.Linq;
using VacationRental.Domain.Bookings;

namespace VacationRental.Application.Common.Services
{
    public class BookingSearchService : IBookingSearchService
    {
        public IEnumerable<BookingModel> GetBookingsByDay(GetBookingsByDayDTO model)
        {
            return model.Bookings.Where(book => 
                book.Start.Date <= model.Day && book.End.Date.AddDays(model.PreparationTime) > model.Day);
        }

        public IEnumerable<BookingModel> GetBookingsByRangeOfTime(GetBookingsByRangeOfTimeDTO model)
        {
            var bookings = model.Bookings.Where(book =>
                book.Start.Date < model.Start.Date.AddDays(model.Nights) &&
                book.Start.Date.AddDays(book.Nights + model.PreparationTime) > model.Start.Date);

            return bookings;
        }
    }
}