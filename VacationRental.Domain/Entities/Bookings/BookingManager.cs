using System;
using System.Collections;
using System.Collections.Generic;
using VacationRental.Domain.Models.Bookings;

namespace VacationRental.Domain.Entities.Bookings
{
    public static class BookingManager
    {
        public static bool isBookingAvailable(IReadOnlyList<Booking> bookings, DateTime start, int nights, int units, int preparationTimeInDays)
        {
            var numberOfBookings = 0;

            for (var i = 0; i < nights; i++)
            {                
                numberOfBookings = CalculateNumberOfAvailableBookings(bookings, start, nights, preparationTimeInDays);                
            }

            if (numberOfBookings >= units)
                return false;

            return true;
        }
        
        public static int CalculateNumberOfAvailableBookings(IReadOnlyList<Booking> bookings, DateTime start, int nights, int preparationTimeInDays)
        {
            var numberOfAvailableBookings = 0;

            foreach (var booking in bookings)
            {
                if (booking.ShouldAddNewBookingToRental(start, nights, preparationTimeInDays))
                    numberOfAvailableBookings++;
            }

            return numberOfAvailableBookings;
        }
    }
}
