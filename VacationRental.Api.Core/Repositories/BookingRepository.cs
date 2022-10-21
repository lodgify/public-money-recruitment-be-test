using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Core.Helpers;
using VacationRental.Api.Core.Interfaces;
using VacationRental.Api.Core.Models;

namespace VacationRental.Api.Core.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingViewModel> _bookings;

        public BookingRepository(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings)
        {
            _bookings = bookings;
            _rentals = rentals;
        }

        public BookingViewModel GetBooking(int bookingId)
        {
            if (!_bookings.ContainsKey(bookingId))
                throw new ApplicationException("Booking not found");

            return _bookings[bookingId];
        }

        public ResourceIdViewModel InsertNewBooking(BookingBindingModel newBooking)
        {
            if (newBooking.Nights <= 0)
                throw new ApplicationException("Nights must be positive");
            if (!_rentals.ContainsKey(newBooking.RentalId))
                throw new ApplicationException("Rental not found");

            // checks every rental available for the night
            for (var i = 0; i < newBooking.Nights; i++)
            {
                var occupiedUnits = 0;
                foreach (var booking in _bookings.Values)
                {
                    if (CommonHelper.CheckOccupancyAvailability(booking, newBooking, _rentals[newBooking.RentalId].PreparationTimeInDays))
                        occupiedUnits++;
                }

                if (occupiedUnits >= _rentals[newBooking.RentalId].Units)
                    throw new ApplicationException("Not available");
            }

            var resource = _bookings.CreateResourceIdForBookings();

            _bookings.Add(resource.Id, new BookingViewModel
            {
                Id = resource.Id,
                Nights = newBooking.Nights,
                RentalId = newBooking.RentalId,
                Start = newBooking.Start.Date
            });

            return resource;
        }
    }
}
