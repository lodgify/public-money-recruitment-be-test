using System;
using System.Collections.Generic;
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

        public ResourceIdViewModel InsertNewBooking(BookingBindingModel bookingModel)
        {
            if (bookingModel.Nights <= 0)
                throw new ApplicationException("Nights must be positive");
            if (!_rentals.ContainsKey(bookingModel.RentalId))
                throw new ApplicationException("Rental not found");

            for (var i = 0; i < bookingModel.Nights; i++)
            {
                var count = 0;
                foreach (var booking in _bookings.Values)
                {
                    if (CommonHelper.ValidateBookingDates(booking, bookingModel))
                        count++;
                }
                if (count >= _rentals[bookingModel.RentalId].Units)
                    throw new ApplicationException("Not available");
            }

            var resource = _bookings.CreateResourceIdForBookings();

            _bookings.Add(resource.Id, new BookingViewModel
            {
                Id = resource.Id,
                Nights = bookingModel.Nights,
                RentalId = bookingModel.RentalId,
                Start = bookingModel.Start.Date
            });

            return resource;
        }
    }
}
