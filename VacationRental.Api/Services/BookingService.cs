using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Data.Interfaces;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRentalRepository _rentalRepository;

        public BookingService(
            IRentalRepository rentalRepository,
            IBookingRepository bookingRepository)
        {
            _rentalRepository = rentalRepository;
            _bookingRepository = bookingRepository;
        }

        public int Create(BookingBindingModel currentBooking)
        {
            if (currentBooking.Nights <= 0)
                throw new ApplicationException("You must provide a positive number of nights");

            if (currentBooking.Start < DateTime.Now)
                throw new ApplicationException("You must book something ahead.");

            if (!_rentalRepository.HasValue(currentBooking.RentalId))
                throw new ApplicationException("404 - Rental not found");

            var existingBookings = _bookingRepository.GetBookingsByRentalId(currentBooking.RentalId);
            var units = _rentalRepository.Get(currentBooking.RentalId).Units;

            var blockedUnits = GetAvailableUnits(existingBookings, currentBooking, units);

            if (blockedUnits >= units)
                throw new ApplicationException("Not available");

            return _bookingRepository.Insert(new BookingViewModel
            {
                Nights = currentBooking.Nights,
                RentalId = currentBooking.RentalId,
                Start = currentBooking.Start.Date,
                Unit = blockedUnits + 1,
            });
        }

        public BookingViewModel Get(int bookingId)
        {
            if (!_bookingRepository.HasValue(bookingId))
                throw new ApplicationException("404 - Booking not found");

            return _bookingRepository.Get(bookingId);
        }

        private int GetAvailableUnits(IEnumerable<BookingViewModel> bookings, BookingBindingModel currentBooking, int rentalUnits)
        {
            return bookings.Count(
                    p => currentBooking.Start < p.Start &&
                    currentBooking.End >= p.Start ||
                    currentBooking.Start > p.Start && currentBooking.Start < p.End);
        }
    }
}
