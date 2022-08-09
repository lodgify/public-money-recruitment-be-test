using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Models;

namespace VacationRental.Api.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IDictionary<int, BookingViewModel> _bookings;

        public BookingRepository(IDictionary<int, BookingViewModel> bookings)
        {
            _bookings = bookings;
        }

        public int BookingCount() => _bookings.Keys.Count;

        public BookingViewModel GetBooking(int id) => _bookings.FirstOrDefault(x => x.Key == id).Value;

        public int CreateBooking(BookingViewModel model)
        {
            _bookings.Add(model.Id, model);

            return model.Id;
        }
    }
}