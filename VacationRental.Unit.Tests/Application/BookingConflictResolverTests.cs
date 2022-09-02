using System;
using System.Linq;
using VacationRental.Unit.Tests.Stubs;
using VR.Application.Resolvers;
using VR.Domain.Models;
using VR.Infrastructure.Exceptions;
using Xunit;

namespace VacationRental.Unit.Tests.Application
{
    public class BookingConflictResolverTests
    {
        private readonly IBookingConflictResolver _bookingConflictResolver;

        public BookingConflictResolverTests()
        {
            _bookingConflictResolver = new BookingConflictResolver();
        }

        [Fact]
        public void GetAvailableUnit_ShouldReturnAvailableUnitNumber()
        {
            var rental = new Rental()
            {
                PreparationTimeInDays = 2,
                Units = 4
            };

            var availableUnit = _bookingConflictResolver.GetAvailableUnit(rental, BookingStubs.GetRentalBookingsWithThirdUnitAvailable());

            Assert.Equal(3, availableUnit);
        }

        [Fact]
        public void GetAvailableUnit_ShouldReturnExceptionIfNoAvailableUnits()
        {
            var rental = new Rental()
            {
                PreparationTimeInDays = 2,
                Units = 4
            };

            Assert.Throws<BookingConflictsException>(() => _bookingConflictResolver.GetAvailableUnit(rental, BookingStubs.GetRentalBookingsWithoutAvailableUnit()));
        }

        [Fact]
        public void GetCrossBookedUnits_ShouldReturnCrossedBookings()
        {
            var rental = new Rental()
            {
                Id = 1,
                PreparationTimeInDays = 4,
                Units = 10
            };

            var currentBooking = new Booking()
            {
                Id = 1,
                RentalId = 1,
                Start = new DateTime(2022, 9, 10),
                Nights = 5
            };

            var crossedBookings = _bookingConflictResolver.GetCrossBookedUnits(rental, currentBooking, BookingStubs.GetRentalPossibleCrossBookedUnits()).ToList();

            Assert.Equal(BookingStubs.GetRentalCrossBookedUnits().Select(booking => booking.Id), crossedBookings.Select(booking => booking.Id));
        }

        [Fact]
        public void HasBookingConflicts_ShouldReturnTrueIfUnitsNumberNotEnought()
        {
            var rental = new Rental()
            {
                Id = 1,
                PreparationTimeInDays = 1,
                Units = 1
            };

            var hasConflict = _bookingConflictResolver.HasBookingConflicts(rental, BookingStubs.GetRentalConflictBookings());

            Assert.True(hasConflict);
        }

        [Fact]
        public void HasBookingConflicts_ShouldReturnTrueIfPreparationDaysConflict()
        {
            var rental = new Rental()
            {
                Id = 1,
                PreparationTimeInDays = 4,
                Units = 2
            };

            var hasConflict = _bookingConflictResolver.HasBookingConflicts(rental, BookingStubs.GetRentalConflictBookings());

            Assert.True(hasConflict);
        }

        [Fact]
        public void HasBookingConflicts_ShouldReturnFalseIfNoBookingConflicts()
        {
            var rental = new Rental()
            {
                Id = 1,
                PreparationTimeInDays = 2,
                Units = 2
            };

            var hasConflict = _bookingConflictResolver.HasBookingConflicts(rental, BookingStubs.GetRentalConflictBookings());

            Assert.False(hasConflict);
        }
    }
}
