using System;
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

        public BookingViewModel GetBooking(int id) =>
            _bookings.FirstOrDefault(x => x.Key == id).Value;

        public int CreateBooking(BookingViewModel model)
        {
            _bookings.Add(model.Id, model);

            return model.Id;
        }

        public bool HasRentalAvailable(int rentalId, DateTime date) =>
            _bookings.Values.Any(
                x => x.RentalId == rentalId && x.Start <= date && x.Start.AddDays(x.Nights) > date
            );

        public BookingViewModel[] GetAll() => _bookings.Values.ToArray();
    }
}
