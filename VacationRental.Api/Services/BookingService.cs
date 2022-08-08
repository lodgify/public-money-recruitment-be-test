using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public class BookingService : IBookingService
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingViewModel> _bookings;

        public BookingService(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
        }

        public ResourceIdViewModel AddBooking(BookingBindingModel currentBooking)
        {
            if (currentBooking.Nights <= 0)
                throw new ApplicationException("Nigts must be positive");

            if (currentBooking.Start < DateTime.Now)
                throw new ApplicationException("Booking must be in future");

            if (!_rentals.ContainsKey(currentBooking.RentalId))
                throw new ApplicationException("Rental not found");

            var existingBookings = _bookings.Values.Where(p => p.RentalId == currentBooking.RentalId);
            var units = _rentals[currentBooking.RentalId].Units;

            var blockedUnits = GetAvailableUnits(existingBookings, currentBooking, units);

            if (blockedUnits >= units)
                throw new ApplicationException("Not available");

            var key = new ResourceIdViewModel { Id = _bookings.Keys.Count + 1 };

            _bookings.Add(key.Id, new BookingViewModel
            {
                Id = key.Id,
                Nights = currentBooking.Nights,
                RentalId = currentBooking.RentalId,
                Start = currentBooking.Start.Date,
                Unit = blockedUnits + 1,
            });

            return key;
        }

        public BookingViewModel GetBooking(int bookingId)
        {
            if (!_bookings.ContainsKey(bookingId))
                throw new ApplicationException("Booking not found");

            return _bookings[bookingId];
        }

        private int GetAvailableUnits(IEnumerable<BookingViewModel> bookings, BookingBindingModel currentBooking, int rentalUnits)
        {
            int count = 0;
            foreach (var booking in bookings)
            {
                if ((currentBooking.Start < booking.Start && currentBooking.End >= booking.Start) ||
                    currentBooking.Start > booking.Start && currentBooking.Start < booking.End)
                    count++;
            }
            return count;
        }
    }
}
