using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Events.Rental;
using VacationRental.Domain.Exceptions;
using VacationRental.Domain.Repositories;
using VacationRental.Domain.Services;

namespace VacationRental.Application.Services
{
    public class RentalUpdatedEventHandler : IRentalUpdatedEventHandler
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ILogger<RentalUpdated> _logger;
        public RentalUpdatedEventHandler(IBookingRepository bookingRepository, ILogger<RentalUpdated> logger)
        {
            _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(RentalUpdated eventDetails)
        {
            var bookings = await _bookingRepository.GetByRentalId(eventDetails.RentalId);
            UpdatePreparationTimePeriod(bookings, eventDetails.PreparationTimeIdDays);

            CheckOverlappingDueToUnitsDecreasing(bookings, eventDetails.Units);
            CheckOverlappingDueToPreparationIncreasing(bookings, eventDetails);

            _logger.LogInformation($"Bookings are updated successfully according to the event", eventDetails);
        }

        private void UpdatePreparationTimePeriod(IReadOnlyCollection<Booking> bookings, int preparationTimesInDays)
        {
            foreach (var booking in bookings)
            {
                booking.UpdatePreparationTime(preparationTimesInDays);
            }
        }

        private void CheckOverlappingDueToUnitsDecreasing(IReadOnlyCollection<Booking> bookings, int units)
        {
            var maxBookingDate = bookings.Max(booking => booking.GetEndOfBooking());
            var minBookingDate = bookings.Min(booking => booking.GetStartOfBooking());

            for (var currentDate = minBookingDate; currentDate <= maxBookingDate; currentDate = currentDate.AddDays(1))
            {
                var countOfBlockedUnits = bookings.Count(booking =>
                    booking.WithinBookingPeriod(currentDate) || booking.WithinPreparationPeriod(currentDate));

                if (countOfBlockedUnits > units)
                {
                    throw new DecreasingNumberOfUnitsFailedException();
                }
            }
        }

        private void CheckOverlappingDueToPreparationIncreasing(IReadOnlyCollection<Booking> bookings, RentalUpdated eventDetails)
        {
            foreach (var currentBooking in bookings)
            {
                var numberOfOverlappedBookings = bookings.Count(booking => booking.IsOverlapped(currentBooking));
                if (numberOfOverlappedBookings > eventDetails.Units)
                {
                    throw new IncreasingPreparationTimeFailedException();
                }
            }
        }
    }
}
