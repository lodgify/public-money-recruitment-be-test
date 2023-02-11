using System;
using VacationRental.Domain.Models.Bookings;

namespace VacationRental.Domain.Entities.Rentals
{
    public static class RentalManager
    {
        public static bool HasPreparationDaysAnyConflicts(DateTime start, DateTime nextBookingStart, int nights, int numberOfDifferentBookings, int units, int preparationTimeInDays)
        {            
            if (numberOfDifferentBookings >= units)
            {
                var endDateTimeBooking = start.AddDays(nights + preparationTimeInDays);

                if (endDateTimeBooking > nextBookingStart)
                    return true;
            }

            return false;
        }

        public static bool HasUnitsAnyConflict(Booking booking, int preparationTimeInDays, int units, int numberOfBookings)
        {            
            if (booking.ShouldAddNewBookingToRental(booking.Start, booking.Nights, preparationTimeInDays))
                numberOfBookings++;

            if (numberOfBookings > units)
                return true;
            
            return false;
        }
    }
}
