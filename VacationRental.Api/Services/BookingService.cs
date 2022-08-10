using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.DAL.Interfaces;
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

        public ResourceIdViewModel Create(BookingBindingModel currentBooking)
        {
            if (currentBooking.Nights <= 0)
                throw new ApplicationException("Nights must be positive");

            if (currentBooking.Start < DateTime.Now)
                throw new ApplicationException("Booking must be in future");

            if (!_rentalRepository.HasValue(currentBooking.RentalId))
                throw new ApplicationException("Rental not found");

            var existingBookings = _bookingRepository.GetBookingsByRentalId(currentBooking.RentalId);
            var units = _rentalRepository.Get(currentBooking.RentalId).Units;

            var blockedUnits = GetAvailableUnits(existingBookings, currentBooking, units);

            if (blockedUnits >= units)
                throw new ApplicationException("Not available");

            var key = new ResourceIdViewModel { Id = _bookingRepository.Count + 1 };

            _bookingRepository.Add(key.Id, new BookingViewModel
            {
                Id = key.Id,
                Nights = currentBooking.Nights,
                RentalId = currentBooking.RentalId,
                Start = currentBooking.Start.Date,
                Unit = blockedUnits + 1,
            });

            return key;
        }

        public BookingViewModel Get(int bookingId)
        {
            if (!_bookingRepository.HasValue(bookingId))
                throw new ApplicationException("Booking not found");

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
